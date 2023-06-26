using F88.Digital.Domain.Entities.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.CacheRepositories.AppPartner
{
    public interface IProductCacheRepository
    {
        Task<List<Product>> GetCachedListAsync();

        Task<Product> GetByIdAsync(int brandId);
    }
}