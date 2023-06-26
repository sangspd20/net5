using F88.Digital.Application.Interfaces.Repositories.Share;
using F88.Digital.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.Share
{
    public class ShareRepositoryAsync<T> : IShareRepositoryAsync<T> where T : class
    {
        private readonly AffiliateDbContext _dbContextAffiliate;

        public ShareRepositoryAsync(AffiliateDbContext dbContextAffiliate)
        {
            _dbContextAffiliate = dbContextAffiliate;
        }

        public async Task<List<T>> GetAllAffiliateAsync()
        {
            return await _dbContextAffiliate
                .Set<T>()
                .ToListAsync();
        }


    }
}
