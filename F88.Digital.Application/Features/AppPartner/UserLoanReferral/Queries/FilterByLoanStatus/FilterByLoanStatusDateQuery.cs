using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries.FilterByLoanStatus
{
    public class FilterByLoanStatusDateQuery : IRequest<Result<SummaryUserLoanRefResponse>>
    {
        public int UserProfileId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ToDate { get; set; }

        public int LoanStatus { get; set; }

        public string PhoneNumber { get; set; }

        public class FilterByLoanStatusDateQueryHandler : IRequestHandler<FilterByLoanStatusDateQuery, Result<SummaryUserLoanRefResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IMapper _mapper;

            public FilterByLoanStatusDateQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
            }

            public async Task<Result<SummaryUserLoanRefResponse>> Handle(FilterByLoanStatusDateQuery request, CancellationToken cancellationToken)
            {
                var userLoans = await _userLoanReferralRepository.UserLoans
                   .Where(x => x.CreatedOn.Date >= request.FromDate.Date
                            && x.CreatedOn.Date <= request.ToDate.Date
                            && x.UserProfileId == request.UserProfileId)
                   .OrderByDescending(x => x.CreatedOn)
                   .Include(x => x.Deposit)
                   .ToListAsync();

                var userLoansFilter = userLoans;

                if (!string.IsNullOrEmpty(request.PhoneNumber))
                {
                    userLoansFilter = userLoans.Where(x => x.PhoneNumber.Contains(request.PhoneNumber)).ToList();
                }

                if(request.LoanStatus != -1)
                {
                    if (request.LoanStatus == ApiConstants.LoanStatus.APPROVED_QFORM || request.LoanStatus == ApiConstants.LoanStatus.PENDING)
                    {
                        userLoansFilter = userLoans.Where(x => x.LoanStatus == ApiConstants.LoanStatus.APPROVED_QFORM || x.LoanStatus == ApiConstants.LoanStatus.PENDING).ToList();
                    }
                    else
                    {
                        userLoansFilter = userLoans.Where(x => x.LoanStatus == request.LoanStatus).ToList();
                    }

                }

                var lstUserLoanGroupByDate = userLoansFilter.GroupBy(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
                                                        .Select(s => new UserLoanRefGroupByDateResponse
                                                        {
                                                            CreatedOn = s.Key.Substring(0, 5),
                                                            UserLoanRefResponses = _mapper.Map<List<UserLoanRefResponse>>(s.ToList())
                                                        })
                                                        .ToList();

                var numberOfPending = userLoans.Count(x => x.LoanStatus == ApiConstants.LoanStatus.PENDING || x.LoanStatus == ApiConstants.LoanStatus.APPROVED_QFORM);
                var numberOfApproved = userLoans.Count(x => x.LoanStatus == ApiConstants.LoanStatus.APPROVED);
                var numberOfCancel = userLoans.Count(x => x.LoanStatus == ApiConstants.LoanStatus.CANCEL);

                var summaryUserLoanResponse = new SummaryUserLoanRefResponse
                {
                    NumberOfPending = numberOfPending,
                    NumberOfApproved = numberOfApproved,
                    NumberOfCancel = numberOfCancel,
                    ListUserLoanRefResponse = lstUserLoanGroupByDate
                };

                return await Result<SummaryUserLoanRefResponse>.SuccessAsync(summaryUserLoanResponse);
            }
        }
    }
}
