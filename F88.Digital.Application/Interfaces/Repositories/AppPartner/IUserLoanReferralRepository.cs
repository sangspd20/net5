using AspNetCoreHero.Results;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IUserLoanReferralRepository
    {
        IQueryable<UserLoanReferral> UserLoans { get; }

        Task<UserLoanReferral> GetByIdAsync(int id);

        Task<int> InsertAsync(UserLoanReferral userLoanReferral);

        Task UpdateAsync(UserLoanReferral userLoanReferral);

        Task<List<UserLoanReferral>> GetListUserLoanByCurrentMonthAsync(int userProfileId);

        Task<List<UserLoanReferral>> FilterListUserLoanByDateAsync(int userProfileId, DateTime fromDate, DateTime toDate);

        Task<List<UserLoanReferral>> FilterListUserLoanByLoanStatusAsync(int userProfileId, int loanStatus);

        Task<UserLoanReferral> GetByTransactionIdAsync(string transactionId);

        Task<GetBalanceByUserResponse> GetBalanceByUserAsync(int userProfileId);

        Task<GetBalanceByUserResponse> GetBalanceByUserInCurrentMonthAsync(int userProfileId);

        GetBalanceByUserResponse GetBalanceByUserInMonth(int userProfileId,int month, int year);

        Task<List<UserLoanReferral>> GetListUserLoanByDateAsync(string userPhone, DateTime fromDate, DateTime toDate);
        Task<ResponseApiData> SendLadipageAffiliate(string url, SendAffiliateModel sendAffiliate);
    }
}
