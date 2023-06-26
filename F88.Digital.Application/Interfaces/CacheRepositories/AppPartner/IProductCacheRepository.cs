using F88.Digital.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.CacheRepositories.AppPartner
{
    public interface IBrandCacheRepository
    {
        Task<List<Brand>> GetCachedListAsync();

        Task<Brand> GetByIdAsync(int brandId);
    }
}