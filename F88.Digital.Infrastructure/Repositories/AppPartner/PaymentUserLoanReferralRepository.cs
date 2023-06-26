using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using F88.Digital.Domain.Entities.AppPartner;
using F88.Digital.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Infrastructure.Repositories.AppPartner
{
    public class PaymentUserLoanReferralRepository : IPaymentUserLoanReferralRepository
    {
        private readonly IRepositoryAsync<PaymentUserLoanReferral> _repository;
        private readonly IDistributedCache _distributedCache;
        private readonly AppPartnerDbContext _context;
        public PaymentUserLoanReferralRepository(IDistributedCache distributedCache, IRepositoryAsync<PaymentUserLoanReferral> repository, AppPartnerDbContext context)
        {
            this._distributedCache = distributedCache;
            this._repository = repository;
            _context = context;
        }

        public IQueryable<PaymentUserLoanReferral> PaymentUserLoanReferrals => _repository.Entities;
        public async Task<PaymentUserLoanReferral> InsertAsync(PaymentUserLoanReferral paymentUserLoanReferral)
        {
            var item = await _context.PaymentUserLoanReferral.AddAsync(paymentUserLoanReferral);
            _context.SaveChanges();
            return item.Entity;
        }

        public async Task InsertRangeAsync(List<PaymentUserLoanReferral> paymentUserLoanReferral)
        {
            await _repository.AddRangeAsync(paymentUserLoanReferral);
        }

        public Task UpdateAsync(PaymentUserLoanReferral paymentUserLoanReferral)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PaymentHistoryDetail> GetPaymentHistoryDetail(string userphone, int paymentId)
        {
            return (from paymentUserLoanReferral in _context.PaymentUserLoanReferral.Include(x => x.Payment).ThenInclude(x => x.UserBank).ThenInclude(x => x.Bank)                         
                         where paymentUserLoanReferral.UserLoanReferral.UserProfile.UserPhone == userphone && paymentUserLoanReferral.PaymentId == paymentId
                         select new PaymentHistoryDetail
                         {
                             PhoneNumber = paymentUserLoanReferral.UserLoanReferral.PhoneNumber,
                             AccountNumber = paymentUserLoanReferral.Payment.UserBank.AccNumber,
                             CurrentMoney = paymentUserLoanReferral.Payment.PaidValue,
                             BankCode = paymentUserLoanReferral.Payment.UserBank.Bank.Code,
                             OtherAmount = paymentUserLoanReferral.Payment.OtherAmount,
                             NetMoney = paymentUserLoanReferral.Payment.OtherAmount,
                             TransferDate = paymentUserLoanReferral.Payment.TransferDate.Value.ToString("hh:mm - dd/MM/yyyy"),
                             TaxValue = paymentUserLoanReferral.Payment.TaxValue != 0 ? (paymentUserLoanReferral.Payment.PaidValue * paymentUserLoanReferral.Payment.TaxValue) / 100 : paymentUserLoanReferral.Payment.PaidValue,
                             Status = paymentUserLoanReferral.Payment.Status,
                             Notes = paymentUserLoanReferral.Payment.Notes
                             
                         }).AsQueryable();             
        }
    }
}
