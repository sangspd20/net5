using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.CacheKeys.AppPartner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using AspNetCoreHero.Extensions.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class UserBankRepository : IUserBankRepository
    {
        private readonly IRepositoryAsync<UserBank> _repository;
        private readonly IRepositoryAsync<Bank> _bankRepository;

        private readonly IDistributedCache _distributedCache;
        public UserBankRepository(IRepositoryAsync<UserBank> repository, IRepositoryAsync<Bank> bankRepository, IDistributedCache distributedCache)
        {
            this._repository = repository;
            this._bankRepository = bankRepository;
            _distributedCache = distributedCache;
        }

        public IQueryable<UserBank> UserBanks => _repository.Entities;

        public IQueryable<Bank> Banks => _bankRepository.Entities;

        public async Task ActiveUserBankAsync(int userBankId, int userProfileId, UserBank userBank)
        {
            if (userBank != null)
            {
                var lstUserBank = await GetListBanksByUserAsync(userProfileId);
                var lstOther = lstUserBank.Where(x => x.AccNumber != userBank.AccNumber).ToList();

                foreach (var item in lstOther)
                {
                    item.UserBankStatus = false;
                    var result = _repository.UpdateAsync(item);
                }

                await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBankId(userBank.Id));
                await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBank(userBank.UserProfileId));
            }

        }

        public async Task DeleteAsync(UserBank userBank)
        {
            await _repository.DeleteAsync(userBank);
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBankId(userBank.Id));
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBank(userBank.UserProfileId));
        }

        public async Task<UserBank> GetByAccNumberAsync(string accNumber)
        {
            string cacheKey = UserProfileCacheKeys.GetKeyUserBankAccNumber(accNumber);
            var userBank = await _distributedCache.GetAsync<UserBank>(cacheKey);

            if (userBank == null)
            {
                userBank = UserBanks.Where(x => x.AccNumber == accNumber).FirstOrDefault();

                await _distributedCache.SetAsync(cacheKey, userBank);
            }

            return userBank;
        }

        public async Task<UserBank> GetByIdAsync(int id)
        {
            string cacheKey = UserProfileCacheKeys.GetKeyUserBankId(id);
            var userBank = await _distributedCache.GetAsync<UserBank>(cacheKey);

            if (userBank == null)
            {
                userBank = UserBanks.Where(x => x.Id == id).FirstOrDefault();

                await _distributedCache.SetAsync(cacheKey, userBank);
            }

            return userBank;
        }

        public async Task<List<UserBank>> GetListBanksByUserAsync(int userProfileId)
        {
            //string cacheKey = UserProfileCacheKeys.GetKeyUserBank(userProfileId);
            //var userBanklst = await _distributedCache.GetAsync<List<UserBank>>(cacheKey);

            //if(userBanklst == null)
            //{ 
            //        userBanklst = await UserBanks.Include(x => x.Bank)
            //                                     .Where(x => x.UserProfileId == userProfileId)
            //                                     .ToListAsync();

            //  await  _distributedCache.SetAsync(cacheKey, userBanklst);
            //}
            var userBanklst = await UserBanks.Include(x => x.Bank)
                                           .Where(x => x.UserProfileId == userProfileId)
                                           .ToListAsync();
            return userBanklst;
        }

        public async Task<int> InsertAsync(UserBank userBank)
        {
            await _repository.AddAsync(userBank);
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBank(userBank.UserProfileId));
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBankId(userBank.Id));
            return userBank.Id;
        }

        public async Task<bool> IsExistAccNumberAsync(string accNumber)
        {
            var exists = UserBanks.Any(x => x.AccNumber == accNumber);
            return await Task.FromResult(exists);
        }

        public async Task UpdateAsync(UserBank userBank)
        {
            await _repository.UpdateAsync(userBank);
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBankId(userBank.Id));
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserBank(userBank.UserProfileId));

        }
    }
}
