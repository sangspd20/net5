using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.CacheRepositories.AppPartner
{
    public interface IBaseEntityCacheRepository<T>
    {
        Task<List<T>> GetCachedListAsync();

        Task<T> GetByIdAsync(int entityId);
    }
}
