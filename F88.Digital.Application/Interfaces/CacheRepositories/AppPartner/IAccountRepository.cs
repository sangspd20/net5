using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.CacheRepositories.AppPartner
{
    public interface IAccountRepository
    {
        Task<UserProfile> LoginAsync(string userPhone, string password);
        Task<int> InsertOTPAsync(UserOTP user);
        Task<VerifyOTPResponse> VerifyOTPAsync(string userPhone, string otp, string deviceId, int expiredOTP);
        Task<VerifyOTPResponse> VerifyOTPShareServiceAsync(string url, string apiKey, string refId, string otp);
    }
}
