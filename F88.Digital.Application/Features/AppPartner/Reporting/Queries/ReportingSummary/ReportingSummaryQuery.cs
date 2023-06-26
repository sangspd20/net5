using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Reporting.Queries.ReportingSummary
{
   public class ReportingSummaryQuery : IRequest<Result<ReportingSummaryResponse>>
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class ReportingSummaryQueryHandler : IRequestHandler<ReportingSummaryQuery, Result<ReportingSummaryResponse>>
    {
        private readonly IUserLoanReferralRepository _userLoanReferralRepository;
        private readonly  IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        public ReportingSummaryQueryHandler(IUserLoanReferralRepository userLoanReferralRepository,  IMapper mapper, IPaymentRepository paymentRepository)
        {
            _userLoanReferralRepository = userLoanReferralRepository;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
        }

        public async Task<Result<ReportingSummaryResponse>> Handle(ReportingSummaryQuery request, CancellationToken cancellationToken)
        {
            DateTime fromDate = new DateTime(request.Year, request.Month, 01);
            DateTime toDate = new DateTime(request.Year, request.Month, DateTime.DaysInMonth(request.Year, request.Month));

            var lstPayment = await _paymentRepository.Payments
            .Where(x => x.CreatedOn.Date >= fromDate.Date
                        && x.CreatedOn.Date <= toDate.Date && x.Status== 1)
            .ToListAsync();


            var lstUserLoan = await _userLoanReferralRepository.UserLoans
            .Where(x => x.CreatedOn.Date >= fromDate.Date
                        && x.CreatedOn.Date <= toDate.Date)
            .ToListAsync();

            var summaryReporting = new ReportingSummaryResponse()
            {
                SummaryRewardMoney =  (decimal)lstPayment.Sum(x => x.PaidValue),
                SummaryLoanAmount = lstUserLoan.Sum(x => x.LoanAmount)
            };

            return await Result<ReportingSummaryResponse>.SuccessAsync(summaryReporting);
        }
    }

}
