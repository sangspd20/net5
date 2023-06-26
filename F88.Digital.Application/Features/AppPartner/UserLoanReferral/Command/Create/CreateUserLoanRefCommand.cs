using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using UserEntity = F88.Digital.Domain.Entities.AppPartner.UserLoanReferral;
using System.Threading;
using System.Threading.Tasks;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.Shared;
using Microsoft.Extensions.Options;
using F88.Digital.Application.DTOs.Settings;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Constants;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command.Create;
using System;
using System.Linq;
using static F88.Digital.Application.Constants.ApiConstants;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command
{
    public partial class CreateUserLoanRefCommand : BaseRequestModel, IRequest<Result<int>>
    {
        public int UserProfileId { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public int RefTempGroupId { get; set; }

        public int RefRealGroupId { get; set; }

        public int RefContractGroupId { get; set; }

        public string RefAsset { get; set; }
        public string RegionID { get; set; }

        public CreateDepositRequest Deposit { get; set; }
    }

    public class SendAffiliateModel
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("fullName")]

        public string FullName { get; set; }
        [JsonProperty("assetType")]

        public string AssetType { get; set; }
        [JsonProperty("province")]

        public string Province { get; set; }
        [JsonProperty("district")]

        public string District { get; set; }
        [JsonProperty("groupId")]

        public int GroupId { get; set; }
        [JsonProperty("url")]

        public string Url { get; set; }
        [JsonProperty("trackingId")]

        public string TrackingId { get; set; }
        [JsonProperty("utmSource")]

        public string UtmSource { get; set; }
        [JsonProperty("regionId")]

        public int RegionId { get; set; }

    }

    public class CreateUserLoanRefCommandHandler : IRequestHandler<CreateUserLoanRefCommand, Result<int>>
    {
        private readonly IUserLoanReferralRepository _userLoanReferralRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        private AffiliateSettings _affiliateSettings { get; set; }
        public CreateUserLoanRefCommandHandler(IOptions<AffiliateSettings> affiliateSettings, IUserLoanReferralRepository userLoanReferralRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserProfileRepository userProfileRepository)
        {
            _userLoanReferralRepository = userLoanReferralRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _affiliateSettings = affiliateSettings.Value;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<Result<int>> Handle(CreateUserLoanRefCommand request, CancellationToken cancellationToken)
        {
            // get user profile by phone
            var userProfile = await _userProfileRepository.GetByIdAsync(request.UserPhone);

            if (userProfile != null)
            {
                if (!userProfile.Status)
                {
                    return await Result<int>.FailAsync(MessageConstants.LockAccount_Error);
                }
                request.UserProfileId = userProfile.Id;
            }


            request.Deposit.UserProfileId = request.UserProfileId;

            //validate and check phone
            var isPhoneValid = CustomValidationHelper.IsFormatPhone(request.PhoneNumber);
            if (!isPhoneValid)
            {
                return await Result<int>.FailAsync("Số điện thoại không đúng định dạng!");
            }

            #region Rule check 10 lead pending 1 day  and 30 lead pending in 1 week
            DateTime today = DateTime.Today;
            DateTime thisWeekStart = today.StartOfWeek(DayOfWeek.Monday);
            DateTime thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            if(!userProfile.Source.Equals("CTV doanh nghiệp"))
            {
                var userLoanRefInWeek = await _userLoanReferralRepository.GetListUserLoanByDateAsync(request.UserPhone, thisWeekStart, thisWeekEnd);
                if (userLoanRefInWeek.Count > 0)
                {
                    var userLoanRefPendingInWeeks = userLoanRefInWeek.Where(x => x.LoanStatus == LoanStatus.PENDING).ToList();
                    if (userLoanRefPendingInWeeks.Count > 30)
                        return await Result<int>.FailAsync("Đã quá 30 đơn cần xử lý trong 1 tuần.");

                    var userLoanRefPendingInToday = userLoanRefPendingInWeeks.Where(x => x.CreatedOn.Day == today.Day).ToList();
                    if (userLoanRefPendingInToday.Count > 10)
                        return await Result<int>.FailAsync("Đã quá 10 đơn cần xử lý trong 1 ngày.");
                }
            }               
            #endregion

            #region NewCode
            var objAffiliate = new SendAffiliateModel
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                AssetType = request.RefAsset,
                Province = request.Province,
                District = request.District,
                GroupId = request.RefTempGroupId,
                Url = ApiConstants.PartnerCode.APP_PARTNER_LINK,
                TrackingId = Guid.NewGuid().ToString(),
                UtmSource = userProfile.Source.Equals("CTV doanh nghiệp") ? ApiConstants.PartnerCode.APP_PARTNER_COMPANY : ApiConstants.PartnerCode.APP_PARTNER_LINK,
                RegionId = !string.IsNullOrEmpty(request.RegionID) ? int.Parse(request.RegionID) : 0
            };
            var affResult = await _userLoanReferralRepository.SendLadipageAffiliate(_affiliateSettings.AppPartnerApiUrl, objAffiliate);
            #endregion

            #region oldCode
            //object objAffiliate = new
            //{
            //    name = request.FullName,
            //    phone = request.PhoneNumber,
            //    select1 = request.RefAsset,
            //    Province = request.Province,
            //    District = request.District,
            //    CurrentGroupID = request.RefTempGroupId,
            //    link = ApiConstants.PartnerCode.APP_PARTNER_LINK,
            //    ReferenceType = ApiConstants.PartnerCode.APP_PARTNER_CODE,
            //    RegionId = request.RegionID
            //};
            //string apiAffUrl = _affiliateSettings.AppPartnerApiUrl;

            //var affResult = RestApiPerform.RestApiPost(apiAffUrl, objAffiliate);
            #endregion

            if (affResult == null) return await Result<int>.FailAsync("Thất bại");
            

            if(affResult.code != 200) return await Result<int>.FailAsync(affResult.message);

            //Push data to UserLoanReferral Table
            var userLoanRef = _mapper.Map<UserEntity>(request);
            var affResponse = JsonConvert.DeserializeObject<CreateAffiliateResultResponse>(JsonConvert.SerializeObject(affResult.data));
            userLoanRef.TransactionId = objAffiliate.TrackingId;
            userLoanRef.IsF88Cus = true;
            userLoanRef.RefFinalGroupId = userLoanRef.RefRealGroupId = Convert.ToInt32(affResponse.RefRealGroupId);
            userLoanRef.PolId = affResponse.PolId;
            //userLoanRef.TransactionId = affResponse.TransactionId;
            //userLoanRef.IsF88Cus = affResponse.IsF88Cus;
            //userLoanRef.RefFinalGroupId = userLoanRef.RefRealGroupId = Convert.ToInt32(affResponse.RefRealGroupId);
            int num1 = await this._userLoanReferralRepository.InsertAsync(userLoanRef);
            int num2 = await this._unitOfWork.Commit(cancellationToken, request.UserPhone);
            Result<int> result = await Result<int>.SuccessAsync("Đăng ký thành công");
            return result;
        }
    }
}
