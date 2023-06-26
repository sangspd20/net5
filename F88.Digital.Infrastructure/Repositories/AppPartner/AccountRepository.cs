using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using F88.Digital.Application.Helpers;
using F88.Digital.Application.Interfaces.CacheRepositories.AppPartner;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.CacheKeys.AppPartner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IRepositoryAsync<UserOTP> _repositoryUserOTP;
        private readonly IRepositoryAsync<UserProfile> _repositoryUserProfile;

        private readonly IDistributedCache _distributedCache;
        public AccountRepository(IDistributedCache distributedCache, IRepositoryAsync<UserOTP> repositoryUserOTP, IRepositoryAsync<UserProfile> repositoryUserProfile)
        {
            this._distributedCache = distributedCache;
            this._repositoryUserOTP = repositoryUserOTP;
            this._repositoryUserProfile = repositoryUserProfile;
        }

        public async Task<int> InsertOTPAsync(UserOTP user)
        {
            await _repositoryUserOTP.AddAsync(user);
            return user.Id;
        }


        public async Task<UserProfile> LoginAsync(string userPhone, string password)
        {
            var user = await _repositoryUserProfile.Entities
                            .Include(x => x.UserAuthToken)
                            .Include(x=>x.UserBanks)
                            .Where(p => p.UserPhone == userPhone)
                            .FirstOrDefaultAsync();

            if (user == null) return await Task.FromResult(user);
            if(!EncryptHelper.VerifyPassWord(password, user.UserAuthToken.Password)) return await Task.FromResult(user = null);

            return await Task.FromResult(user);
        }


        public async Task<VerifyOTPResponse> VerifyOTPAsync(string userPhone, string otp, string deviceId, int expiredOTP)
        {          
            var userOtp = _repositoryUserOTP.Entities.Where(p => p.UserPhone == userPhone && p.DeviceId == deviceId)
                                                  .OrderByDescending(x => x.CreatedOn)
                                                  .FirstOrDefault();

            var result = new VerifyOTPResponse
            {
                Ok = true,
                Message = "Thành công"
            };

            if (userOtp == null) 
            {
                result = new VerifyOTPResponse
                {
                    Ok = false,
                    Message = "Số điện thoại không tồn tại hoặc không đúng mã thiết bị"
                };

                return await Task.FromResult(result);
            } 

            if(!EncryptHelper.VerifyPassWord(otp, userOtp.OTPHash))
            {
                result = new VerifyOTPResponse
                {
                    Ok = false,
                    Message = "Mã OTP không chính xác"
                };

                return await Task.FromResult(result);
            }

            //Check expired
            DateTime a = userOtp.CreatedOn;
            DateTime b = DateTime.Now;
            TimeSpan span = b - a;

            if (span.Minutes * 60 + span.Seconds > expiredOTP)
            {
                result = new VerifyOTPResponse
                {
                    Ok = false,
                    Message = "Mã OTP đã hết hiệu lực"
                };

                return await Task.FromResult(result);
            }

            return await Task.FromResult(result); 
        }

        public async Task<VerifyOTPResponse> VerifyOTPShareServiceAsync(string url, string apiKey, string refId, string otp)
        {
            var checkOtpShareServiceRequest = new CheckOtpShareServiceRequest
            {
                refId = refId,
                otp = otp
            };
            var client = new RestClient(url);
            var req = new RestRequest("/VerifyOTP", Method.POST);
            req.AddHeader("x-api-key", apiKey);
            req.AddJsonBody(JsonConvert.SerializeObject(checkOtpShareServiceRequest));
            var rs = client.Execute(req);
            var jsonData = JsonConvert.DeserializeObject<CheckOtpShareServiceResponse>(rs.Content);
            if (jsonData == null)
            {
                var result = new VerifyOTPResponse { Ok = false, Message = "Số điện thoại không tồn tại hoặc không đúng mã thiết bị"  };
                return await Task.FromResult(result);
            }
            else
            {
                var result = new VerifyOTPResponse { Ok = jsonData?.error_code == 200 ? true : false, Message = jsonData?.error_message };
                return await Task.FromResult(result);
            }            
        }
    }
}
