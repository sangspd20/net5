using F88.Digital.Domain.Entities.AppPartner;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IUserProfileRepository
    {
        IQueryable<UserProfile> UserProfiles { get; }

        Task DeleteAsync(UserProfile userProfileId);
        Task<UserProfile> GetByIdAsync(string userPhone);
        Task<UserProfile> GetByUserPhoneAsync(string userPhone, string password);
        Task<List<UserProfile>> GetListAsync();
        Task<int> InsertAsync(UserProfile userProfile);
        Task UpdateAsync(UserProfile userProfile);

        Task<bool> IsPassportExist(string passport, string userPhone);

        Task<bool> IsCurrentPasswordCorrect(string userPhone, string password);

        Task<UserProfile> GetByPhoneAndPassportAsync(string userPhone, string passport);

        Task<UserProfile> GetIncludeUserLoanByUserPhoneAsync(string userPhone);

        Task<UserProfile> GetByUserIdAsync(int userProfileId);

        Task<UserProfile> GetUserInfoIncludeUserBankByUserPhoneAsync(string userPhone);
        Task UpdateAgreementConfirmed(int userId);
        int UpdateUserProfile(UserProfile userProfile);
    }
}