using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.CacheKeys.AppPartner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreHero.Extensions.Caching;
using System.Threading.Tasks;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries;
using Newtonsoft.Json;
using RestSharp;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class UserLoanReferralRepository : IUserLoanReferralRepository
    {
        private readonly IRepositoryAsync<UserLoanReferral> _repository;
        private readonly IDistributedCache _distributedCache;

        public UserLoanReferralRepository(IDistributedCache distributedCache, IRepositoryAsync<UserLoanReferral> repository)
        {
            this._distributedCache = distributedCache;
            this._repository = repository;
        }
        public IQueryable<UserLoanReferral> UserLoans => _repository.Entities;

        public async Task<List<UserLoanReferral>> FilterListUserLoanByDateAsync(int userProfileId, DateTime fromDate, DateTime toDate)
        {
            var filterDate = UserLoans.Where(x => x.CreatedOn >= fromDate && x.CreatedOn <= toDate);
            var filterPagingByDate = await QueryableExtensions.ToPaginatedListAsync(filterDate, ApiConstants.PagingInfo.PAGE_NUMBER, ApiConstants.PagingInfo.PAGE_SIZE);

            return filterPagingByDate.Data;
        }

        public Task<List<UserLoanReferral>> FilterListUserLoanByLoanStatusAsync(int userProfileId, int loanStatus)
        {
            throw new NotImplementedException();
        }

        public async Task<GetBalanceByUserResponse> GetBalanceByUserAsync(int userProfileId)
        {
            var user = await UserLoans
                .Include(x => x.Deposit)
                .Where(x => x.UserProfileId == userProfileId && x.Deposit.Status)             
                .ToListAsync();

            var balanceUser = user.GroupBy(x => x.UserProfileId)
                                  .Select(s => new GetBalanceByUserResponse
                                  {
                                      UserProfileId = s.Key,
                                      Balance = s.Sum(s => s.Deposit.BalanceValue)
                                  })
                                  .FirstOrDefault();

            return balanceUser;
        }

        public async Task<GetBalanceByUserResponse> GetBalanceByUserInCurrentMonthAsync(int userProfileId)
        {
            var date = DateTime.Now;
            var user = await UserLoans
                .Include(x => x.Deposit)
                .Where(x => x.UserProfileId == userProfileId && x.Deposit.Status && x.Deposit.CreatedOn.Month == date.Month && x.Deposit.CreatedOn.Year == date.Year)
                .ToListAsync();

            var balanceUser = user.GroupBy(x => x.UserProfileId)
                                  .Select(s => new GetBalanceByUserResponse
                                  {
                                      UserProfileId = s.Key,
                                      Balance = s.Sum(s => s.Deposit.BalanceValue)
                                  })
                                  .FirstOrDefault();

            return balanceUser;
        }

        public GetBalanceByUserResponse GetBalanceByUserInMonth(int userProfileId, int month, int year)
        {
            var user =  UserLoans
                .Include(x => x.Deposit)
                .Where(x => x.UserProfileId == userProfileId && x.Deposit.Status == true && x.Deposit.CreatedOn.Month == month && x.Deposit.CreatedOn.Year == year)
                .ToList();

            var balanceUser = user.GroupBy(x => new { x.UserProfileId,x.Deposit.CreatedOn.Month, x.Deposit.CreatedOn.Year})
                                  .Select(s => new GetBalanceByUserResponse
                                  {
                                      UserProfileId = s.FirstOrDefault().UserProfileId,
                                      Balance = s.Sum(s => s.Deposit.BalanceValue)
                                  })
                                  .FirstOrDefault();

            return balanceUser;
        }

        public async Task<UserLoanReferral> GetByIdAsync(int id)
        {
            var userLoan = await UserLoans.Where(x => x.Id == id)
                                          .Include(x => x.Deposit)
                                          .FirstOrDefaultAsync();
            return userLoan;
        }

        public async Task<UserLoanReferral> GetByTransactionIdAsync(string transactionId)
        {
            var userLoan = await UserLoans.Where(x => x.TransactionId == transactionId)
                                        .Include(x => x.Deposit)
                                        .FirstOrDefaultAsync();
            return userLoan;
        }

        public async Task<List<UserLoanReferral>> GetListUserLoanByCurrentMonthAsync(int userProfileId)
        {

            var lstUserLoans = await UserLoans.Where(x => x.UserProfileId == userProfileId && x.CreatedOn.Month == DateTime.Now.Month)
                                              .Include(x => x.Deposit)
                                              .ToListAsync();
            return lstUserLoans;
        }

        public async Task<List<UserLoanReferral>> GetListUserLoanByDateAsync(string userPhone, DateTime fromDate, DateTime toDate)
        {
            var userLoans = await UserLoans
                .Where(x => x.CreatedBy == userPhone
                            && x.CreatedOn >= fromDate 
                            && x.CreatedOn <= toDate)
                .ToListAsync();

            return userLoans;
        }

        public async Task<int> InsertAsync(UserLoanReferral userLoanReferral)
        {
            await _repository.AddAsync(userLoanReferral);
            await _distributedCache.RemoveAsync(UserLoanReferralCacheKeys.GetKey(userLoanReferral.Id));
            await _distributedCache.RemoveAsync(UserLoanReferralCacheKeys.GetListUserLoanKey(userLoanReferral.UserProfileId));

            return userLoanReferral.Id;
        }

        public async Task UpdateAsync(UserLoanReferral userLoanReferral)
        {
            await _repository.UpdateAsync(userLoanReferral);
            await _distributedCache.RemoveAsync(UserLoanReferralCacheKeys.GetKey(userLoanReferral.Id));
            await _distributedCache.RemoveAsync(UserLoanReferralCacheKeys.GetListUserLoanKey(userLoanReferral.UserProfileId));
        }

        public async Task<ResponseApiData> SendLadipageAffiliate(string url, SendAffiliateModel sendAffiliate)
        {
            var client = new RestClient(url);
            var req = new RestRequest("", Method.POST);
            req.AddJsonBody(JsonConvert.SerializeObject(sendAffiliate));
            req.AddHeader("Content-Type", "application/json");
            var rs = await client.ExecuteAsync(req);
            var jsonData = JsonConvert.DeserializeObject<ResponseApiData>(rs.Content);
            return jsonData;
        }
    }
}
