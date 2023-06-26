using AspNetCoreHero.Results;
using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Interfaces.Repositories.AppPartner
{
    public interface IPaymentUserLoanReferralRepository
    {
        IQueryable<PaymentUserLoanReferral> PaymentUserLoanReferrals { get; }

        Task<PaymentUserLoanReferral> InsertAsync(PaymentUserLoanReferral userLoanReferral);

        Task UpdateAsync(PaymentUserLoanReferral userLoanReferral);

        IQueryable<PaymentHistoryDetail> GetPaymentHistoryDetail(string userphone, int paymentId);

    }
}
