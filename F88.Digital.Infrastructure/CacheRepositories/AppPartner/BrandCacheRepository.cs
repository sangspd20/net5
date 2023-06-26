using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.Catalog;
using F88.Digital.Infrastructure.CacheKeys;
using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;
using F88.Digital.Application.Interfaces.CacheRepositories.AppPartner;

namespace F88.Digital.Infrastructure.CacheRepositories.AppPartner
{
    public class BrandCacheRepository : IBrandCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IBrandRepository _brandRepository;

        public BrandCacheRepository(IDistributedCache distributedCache, IBrandRepository brandRepository)
        {
            _distributedCache = distributedCache;
            _brandRepository = brandRepository;
        }

        public async Task<Brand> GetByIdAsync(int brandId)
        {
            string cacheKey = BrandCacheKeys.GetKey(brandId);
            var brand = await _distributedCache.GetAsync<Brand>(cacheKey);
            if (brand == null)
            {
                brand = await _brandRepository.GetByIdAsync(brandId);
                Throw.Exception.IfNull(brand, "Brand", "No Brand Found");
                await _distributedCache.SetAsync(cacheKey, brand);
            }
            return brand;
        }

        public async Task<List<Brand>> GetCachedListAsync()
        {
            string cacheKey = BrandCacheKeys.ListKey;
            var brandList = await _distributedCache.GetAsync<List<Brand>>(cacheKey);
            if (brandList == null)
            {
                brandList = await _brandRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, brandList);
            }
            return brandList;
        }
    }
}