using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.CacheRepositories.AppPartner
{
    public interface IUserProfileCacheRepository
    {
        Task<List<UserProfile>> GetCachedListAsync();

        Task<UserProfile> GetByIdAsync(int brandId);

        Task<UserProfile> GetByUserAccountAsync(string userPhone, string password);
    }
}