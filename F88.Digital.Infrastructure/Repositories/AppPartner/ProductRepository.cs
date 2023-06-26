using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.Catalog;
using F88.Digital.Infrastructure.CacheKeys.AppPartner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryAsync<Product> _repository;
        private readonly IDistributedCache _distributedCache;

        public ProductRepository(IDistributedCache distributedCache, IRepositoryAsync<Product> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public IQueryable<Product> Products => _repository.Entities;

        public async Task DeleteAsync(Product product)
        {
            await _repository.DeleteAsync(product);
            await _distributedCache.RemoveAsync(ProductCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ProductCacheKeys.GetKey(product.Id));
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            return await _repository.Entities.Where(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Product product)
        {
            await _repository.AddAsync(product);
            await _distributedCache.RemoveAsync(ProductCacheKeys.ListKey);
            return product.Id;
        }

        public async Task UpdateAsync(Product product)
        {
            await _repository.UpdateAsync(product);
            await _distributedCache.RemoveAsync(ProductCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ProductCacheKeys.GetKey(product.Id));
        }
    }
}