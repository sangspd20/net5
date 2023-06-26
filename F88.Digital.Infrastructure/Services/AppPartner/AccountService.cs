using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using F88.Digital.Application.DTOs.Settings;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Features.AppPartner.Contract.Query;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.CacheRepositories.AppPartner;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static F88.Digital.Application.DTOs.AppPartner.UserOtpResult;

namespace F88.Digital.Infrastructure.Services.AppPartner
{
    public class AccountService : IAccountService
    {
        private OTPSettings _otpSettings { get; }
        private SendOTPSettings _sendOTPSettings;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IGroupMenuRepository _groupRepository;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }
        private readonly IAppNotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IAWSS3Repository _AWSS3Repository;
        public AccountService(IOptions<OTPSettings> otpSettings,
            SendOTPSettings sendOTPSettings,
            IAccountRepository accountRepository,
            IUserProfileRepository userProfileRepository,
            IUnitOfWork unitOfWork, IMapper mapper,
            IAppNotificationRepository notificationRepository,
            IUserNotificationRepository userNotificationRepository,
            IGroupMenuRepository groupRepository,
            IAWSS3Repository aWSS3Repository)
        {
            _otpSettings = otpSettings.Value;
            _sendOTPSettings = sendOTPSettings;
            _accountRepository = accountRepository;
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _groupRepository = groupRepository;
            _AWSS3Repository = aWSS3Repository;
        }

        public async Task<Result<UserProfileResponse>> LoginAsync(UserLoginRequest user)
        {
            var userProfile = await _accountRepository.LoginAsync(user.UserPhone, user.Password);
            if (userProfile == null) return await Result<UserProfileResponse>.FailAsync("Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại.");

            if (!userProfile.Status) return await Result<UserProfileResponse>.FailAsync(MessageConstants.LockAccount_Error);
            var groupMenus = _groupRepository.GetListGroupMenuByUserId(userProfile.Id);
            var result = _mapper.Map<UserProfileResponse>(userProfile);
            result.isVerify = false;
            if (!string.IsNullOrEmpty(userProfile.PassportFrontURL)
                && !string.IsNullOrEmpty(userProfile.PassportBackURL)
                && !string.IsNullOrEmpty(userProfile.Passport)
                && userProfile.UserBanks.Any())
            {
                result.isVerify = true;
                await _userProfileRepository.UpdateAgreementConfirmed(userProfile.Id);
            }
            result.PassportFrontURL = !string.IsNullOrEmpty(userProfile.PassportFrontURL) ? (userProfile.PassportFrontURL.Contains("/") ? _AWSS3Repository.PresignURL(userProfile.PassportFrontURL) : userProfile.PassportFrontURL) : userProfile.PassportFrontURL;
            result.PassportBackURL = !string.IsNullOrEmpty(userProfile.PassportBackURL) ? (userProfile.PassportBackURL.Contains("/") ? _AWSS3Repository.PresignURL(userProfile.PassportBackURL) : userProfile.PassportBackURL) : userProfile.PassportBackURL;
            result.AvatarURL = !string.IsNullOrEmpty(userProfile.AvatarURL) ? (userProfile.AvatarURL.Contains("/") ? _AWSS3Repository.PresignURL(userProfile.AvatarURL) : userProfile.AvatarURL) : userProfile.AvatarURL;
            result.UserBanks = userProfile.UserBanks.Where(x => x.UserBankStatus).ToList();
            result.Groups = groupMenus.Groups;
            result.Menus = groupMenus.Menus;
            return await Result<UserProfileResponse>.SuccessAsync(result);
        }
        public async Task<Result<int>> SendOtpShareService(UserOTPRequest user)
        {
            var userProfile = await _userProfileRepository.GetByIdAsync(user.UserPhone);
            if (user.Type == TypeSendOtp.Register)
            {
                if (userProfile != null) return await Result<int>.FailAsync("Số điện thoại đã được đăng ký trước đó.");
            }
            else
            {
                if (userProfile == null) return await Result<int>.FailAsync("Số điện thoại không tồn tại trên hệ thống. Vui lòng kiểm tra lại.");
                if (!userProfile.Status) return await Result<int>.FailAsync(MessageConstants.LockAccount_Error);
            }
            var paramSms = new Dictionary<string, string>()
                        {
                            {"refId", $"GROWTH_PARTNER{user.UserPhone}" },
                             {"transactionType", "GROWTH_PARTNER" },
                              {"received", user.UserPhone },
                               {"receivedChannel", "SMS" },
                        };
            var rs = RestApiPerform.RestApiGetWithHeader<BrandNameOtpResult>($"{_sendOTPSettings.Url}/SendOTP", _sendOTPSettings.ApiKey, paramSms);
            if (rs.IsSent)
            {
                return await Result<int>.SuccessAsync("Gửi OTP thành công");
            }
            return await Result<int>.FailAsync("Gửi OTP thất bại");
        }
        public async Task<Result<VerifyOTPResponse>> CheckOtpShareService(string userPhone, string otp, string deviceId)
        {
            var refId = $"GROWTH_PARTNER{userPhone}";
            var result = await _accountRepository.VerifyOTPShareServiceAsync(_sendOTPSettings.Url, _sendOTPSettings.ApiKey, refId, otp);

            if (result.Ok) return await Result<VerifyOTPResponse>.SuccessAsync(result);

            return await Result<VerifyOTPResponse>.FailAsync("Mã OTP không chính xác hoặc hết hiệu lực!");
        }
        public async Task<Result<int>> SendOTPAsync(UserOTPRequest user)
        {
            #region --Send OTP to Phone--
            Random generator = new Random();
            var otp = generator.Next(0, 999999).ToString("D6");

            string otpMessage = _otpSettings.MessageOTP.Replace("{OTP}", otp);
            string otpApiUrl = _otpSettings.OTPApiUrl.Replace("{phone}", user.UserPhone).Replace("{message}", otpMessage);
            var paramSms = new Dictionary<string, string>()
                        {
                            {"phoneNumber", user.UserPhone },
                            {"message", otpMessage }
                        };
            var rs = RestApiPerform.RestApiGet<BrandNameOtpResult>(otpApiUrl, paramSms);
            #endregion

            #region Save OTP
            if (rs.IsSent)
            {
                if (!string.IsNullOrEmpty(otp)) user.OTP = EncryptHelper.HashPassword(otp);

                var userOTP = _mapper.Map<UserOTP>(user);
                await _accountRepository.InsertOTPAsync(userOTP);
                await _unitOfWork.Commit(default, user.UserPhone);

                return await Result<int>.SuccessAsync("Gửi OTP thành công");
            }

            #endregion

            return await Result<int>.FailAsync("Gửi OTP thất bại");
        }

        public async Task<Result<VerifyOTPResponse>> VerifyOTPAsync(string userPhone, string otp, string deviceId)
        {
            var expiredOTP = Convert.ToInt32(_otpSettings.ExpiredOTP);

            var result = await _accountRepository.VerifyOTPAsync(userPhone, otp, deviceId, expiredOTP);

            if (result.Ok) return await Result<VerifyOTPResponse>.SuccessAsync(result);

            return await Result<VerifyOTPResponse>.FailAsync("Mã OTP không chính xác hoặc hết hiệu lực!");
        }

        public async Task<Result<ContractInfoResponse>> GetContractInfo(string userPhone)
        {
            #region Get info
            var userProfile = await _userProfileRepository.GetUserInfoIncludeUserBankByUserPhoneAsync(userPhone);
            if (userProfile == null) return await Result<ContractInfoResponse>.FailAsync("Tài khoản không tồn tại hoặc đã bị khóa");
            #endregion

            #region get user info property for contract
            var userProfileInfoContact = new UserProfileContractInfo()
            {
                FullName = $"{userProfile.LastName} {userProfile.FirstName}",
                UserPhone = userProfile.UserPhone,
                PassportDate = userProfile.PassportDate.HasValue ? userProfile.PassportDate.Value.ToString("dd/MM/yyyy") : "Chưa cập nhật",
                PassportPlace = userProfile.PassportPlace,
                AccNumber = userProfile.UserBanks.Count > 0 ? userProfile.UserBanks.FirstOrDefault().AccNumber : "Chưa cập nhật",
                BankName = userProfile.UserBanks.Count > 0 ? userProfile.UserBanks.FirstOrDefault().Bank.Name : "Chưa cập nhật",
                BankBranch = userProfile.UserBanks.Count > 0 ? userProfile.UserBanks.FirstOrDefault().Branch : "Chưa cập nhật",
                Passport = userProfile.Passport
            };
            #endregion

            string content = File.ReadAllText("./Upload/File/contract.txt");
            var contractInfoResponse = new ContractInfoResponse()
            {
                UserPhone = userProfile.UserPhone,
                ContractDetail = content
                                                    .Replace("{Day}", DateTime.Now.Day.ToString())
                                                    .Replace("{Month}", DateTime.Now.Month.ToString())
                                                    .Replace("{Year}", DateTime.Now.Year.ToString())
                                                    .Replace("{Month}", DateTime.Now.Month.ToString())
                                                    .Replace("{Fullname}", userProfileInfoContact.FullName.ToUpper())
                                                    .Replace("{Userphone}", userProfileInfoContact.UserPhone)
                                                    .Replace("{Passport}", userProfileInfoContact.Passport)
                                                    .Replace("{Passportdate}", userProfileInfoContact.PassportDate)
                                                    .Replace("{Passportplace}", userProfileInfoContact.PassportPlace)
                                                    .Replace("{Accnumber}", userProfileInfoContact.AccNumber)
                                                    .Replace("{Bankname}", userProfileInfoContact.BankName)
                                                    .Replace("{Bankbranch}", userProfileInfoContact.BankBranch)
            };

            return Result<ContractInfoResponse>.Success(contractInfoResponse);
        }

        public async Task<Result<VerifyOTPResponse>> VerifyOTPReferralAsync(string userPhone, string otp, string deviceId, string lastName, string firstName)
        {
            //int expiredOTP = Convert.ToInt32(this._otpSettings.ExpiredOTP);
            //VerifyOTPResponse result1 = await this._accountRepository.VerifyOTPAsync(userPhone, otp, deviceId, expiredOTP);
            Random generator = new Random();
            string randomPassword = generator.Next(0, 99999999).ToString("D8");
            string passwordHash = EncryptHelper.HashPassword(randomPassword);
            UserProfile userProfile = new UserProfile()
            {
                UserPhone = userPhone,
                FirstName = firstName,
                LastName = lastName,
                Status = true,
                UserAuthToken = new UserAuthToken()
                {
                    Password = passwordHash
                }
            };
            int num1 = await this._userProfileRepository.InsertAsync(userProfile);
            int isInsertAccountSuccess = await this._unitOfWork.Commit(new CancellationToken(), userPhone);
            if (isInsertAccountSuccess > 0)
            {
                List<Notification> lstPublicNoti = this._notificationRepository.AppNotifications.Where<Notification>((Expression<Func<Notification, bool>>)(x => x.NotiType == -1 && x.Status)).ToList<Notification>();
                List<UserNotification> lstUserNoti = new List<UserNotification>();
                foreach (Notification notification in lstPublicNoti)
                {
                    Notification pubNoti = notification;
                    UserNotification notiApp = new UserNotification()
                    {
                        UserProfileId = userProfile.Id,
                        AppNotificationId = pubNoti.Id
                    };
                    lstUserNoti.Add(notiApp);
                    notiApp = (UserNotification)null;
                    pubNoti = (Notification)null;
                }
                await this._userNotificationRepository.InsertRangeAsync(lstUserNoti);
                int num2 = await this._unitOfWork.Commit(new CancellationToken(), userPhone);
          //      string otpMessage = this._otpSettings.ReferralMessageOTP.Replace("{UserPhone}", userPhone).Replace("{Password}", randomPassword);
          //      string otpApiUrl = this._otpSettings.OTPApiUrl.Replace("{phone}", userPhone).Replace("{message}", otpMessage);
          //      Dictionary<string, string> paramSms = new Dictionary<string, string>()
          //{
          //  {
          //    "phoneNumber",
          //    userPhone
          //  },
          //  {
          //    "message",
          //    otpMessage
          //  }
          //};
          //      UserOtpResult.BrandNameOtpResult isOptSendSuccess = RestApiPerform.RestApiGet<UserOtpResult.BrandNameOtpResult>(otpApiUrl, paramSms);
          //      if (isOptSendSuccess.IsSent)
          //      {
          //          Result<VerifyOTPResponse> result2 = await Result<VerifyOTPResponse>.SuccessAsync(result1);
          //          return result2;
          //      }
          //      lstPublicNoti = (List<Notification>)null;
          //      lstUserNoti = (List<UserNotification>)null;
          //      otpMessage = (string)null;
          //      otpApiUrl = (string)null;
          //      paramSms = (Dictionary<string, string>)null;
          //      isOptSendSuccess = (UserOtpResult.BrandNameOtpResult)null;
            }
            Result<VerifyOTPResponse> result3 = await Result<VerifyOTPResponse>.FailAsync("Đăng ký tài khoản thất bại");
            return result3;
        }
    }
}
