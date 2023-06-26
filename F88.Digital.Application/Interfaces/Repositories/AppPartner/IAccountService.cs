using AspNetCoreHero.Results;
using F88.Digital.Application.DTOs.AppPartner;
using F88.Digital.Application.DTOs.AppPartner.UserManagement;
using F88.Digital.Application.Features.AppPartner.Contract.Query;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IAccountService
    {
        Task<Result<UserProfileResponse>> LoginAsync(UserLoginRequest user);        
        Task<Result<int>> SendOTPAsync(UserOTPRequest user);
        Task<Result<VerifyOTPResponse>> VerifyOTPAsync(string userPhone, string otp, string deviceId);
        Task<Result<ContractInfoResponse>> GetContractInfo(string userPhone);
        Task<Result<VerifyOTPResponse>> VerifyOTPReferralAsync(
     string userPhone,
     string otp,
     string deviceId,
     string lastName,
     string firstName);

        Task<Result<int>> SendOtpShareService(UserOTPRequest user);
        Task<Result<VerifyOTPResponse>> CheckOtpShareService(string userPhone, string otp, string deviceId);
    }
}
