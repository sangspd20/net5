using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.CacheKeys.AppPartner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreHero.Extensions.Caching;
using F88.Digital.Infrastructure.DbContexts;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IRepositoryAsync<UserProfile> _repository;
        private readonly AppPartnerDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public UserProfileRepository(IDistributedCache distributedCache, IRepositoryAsync<UserProfile> repository, AppPartnerDbContext context)
        {
            this._distributedCache = distributedCache;
            this._repository = repository;
            this._context = context;
        }

        public IQueryable<UserProfile> UserProfiles => _repository.Entities;

        public Task DeleteAsync(UserProfile userProfileId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> GetByIdAsync(string userPhone)
        {
           var userProfile = await UserProfiles.Where(p => p.UserPhone == userPhone)
                                               .Include(x => x.UserAuthToken)
                                               .FirstOrDefaultAsync();
            
            return userProfile;
        }

        public async Task<UserProfile> GetByPhoneAndPassportAsync(string userPhone, string passport)
        {
            var userProfile = await UserProfiles.Where(p => p.UserPhone == userPhone && p.Passport == passport)
                                                .Include(x => x.UserAuthToken)
                                                .FirstOrDefaultAsync();

            return userProfile;
        }

        public async Task<UserProfile> GetByUserIdAsync(int userProfileId)
        {
            var userProfile = await UserProfiles.Where(p => p.Id == userProfileId)
                                                .Include(x => x.UserAuthToken)
                                                .Include(x=>x.UserBanks)
                                                .FirstOrDefaultAsync();

            return userProfile;
        }

        public async Task<UserProfile> GetByUserPhoneAsync(string userPhone, string password)
        {
            string cacheKey = UserProfileCacheKeys.GetKeyUserPhone(userPhone);
            var userProfile = await _distributedCache.GetAsync<UserProfile>(cacheKey);

            if (userProfile == null)
            {
                userProfile = await UserProfiles.Where(p => p.UserPhone == userPhone && p.UserAuthToken.Password == password)
                                                .FirstOrDefaultAsync();

                await _distributedCache.SetAsync(cacheKey, userProfile);
            }

            return userProfile;
        }

        public async Task<UserProfile> GetIncludeUserLoanByUserPhoneAsync(string userPhone)
        {
            var userProfile = await UserProfiles.Where(p => p.UserPhone == userPhone)
                                                .Include(x => x.UserLoanReferrals)
                                                .FirstOrDefaultAsync();

            return userProfile;
        }

        public Task<List<UserProfile>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfile> GetUserInfoIncludeUserBankByUserPhoneAsync(string userPhone)
        {
            var userProfile = await UserProfiles.Where(p => p.UserPhone == userPhone)
                                                .Include(x => x.UserBanks.Where(s => s.UserBankStatus))
                                                .ThenInclude(y => y.Bank)
                                                .FirstOrDefaultAsync();

            return userProfile;
        }

        public async Task<int> InsertAsync(UserProfile userProfile)
        {
            await _repository.AddAsync(userProfile);
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserPhone(userProfile.UserPhone));

            return userProfile.Id;
        }

        public async Task<bool> IsCurrentPasswordCorrect(string userPhone, string password)
        {
            var isPasswordExist = UserProfiles.Include(p => p.UserAuthToken)
                                              .Any(p => p.UserAuthToken.Password == password && p.UserPhone == userPhone);

            return await Task.FromResult(isPasswordExist);
        }

        public async Task<bool> IsPassportExist(string passport, string userPhone)
        {
            var userPassport = UserProfiles.Any(p => p.Passport == passport && p.UserPhone != userPhone);

            return await Task.FromResult(userPassport);
        }

        public async Task UpdateAsync(UserProfile userProfile)
        {
            await _repository.UpdateAsync(userProfile);
            await _distributedCache.RemoveAsync(UserProfileCacheKeys.GetKeyUserPhone(userProfile.UserPhone));
        }
        public int UpdateUserProfile(UserProfile userProfile)
        {
            var rs = _context.Update(userProfile);
            if(rs != null)
            {
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }
        public async Task UpdateAgreementConfirmed(int userId)
        {
            var user = await _repository.GetByIdAsync(userId);
            if(user != null)
            {
                user.IsAgreementConfirmed = true;
                await _repository.UpdateAsync(user);
            }    
        }
    }
}
