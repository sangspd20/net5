using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using F88.Digital.Domain.Entities.AppPartner;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries.FilterByLoanStatus
{
    public class FilterByLoanStatusCurrentMonthQuery : IRequest<Result<SummaryUserLoanRefResponse>>
    {
        public int UserProfileId { get; set; }

        public int LoanStatus { get; set; }

        public int PageSize { get; set; } = ApiConstants.PagingInfo.PAGE_SIZE;

        public class FilterByLoanStatusCurrentMonthQueryHandler : IRequestHandler<FilterByLoanStatusCurrentMonthQuery, Result<SummaryUserLoanRefResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IMapper _mapper;

            public FilterByLoanStatusCurrentMonthQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
            }

            public async Task<Result<SummaryUserLoanRefResponse>> Handle(FilterByLoanStatusCurrentMonthQuery request, CancellationToken cancellationToken)
            {
                var userLoans = await _userLoanReferralRepository.UserLoans
                   .Where(x => x.CreatedOn.Month == DateTime.Now.Month 
                   && x.CreatedOn.Year == DateTime.Now.Year
                   && x.UserProfileId == request.UserProfileId)
                   .OrderByDescending(x => x.CreatedOn)
                   .Include(x => x.Deposit)
                   .ToListAsync();
                var userLoansFilter = userLoans;

                if (request.LoanStatus == ApiConstants.LoanStatus.APPROVED_QFORM || request.LoanStatus == ApiConstants.LoanStatus.PENDING)
                {
                    userLoansFilter = userLoans.Where(x => x.LoanStatus == ApiConstants.LoanStatus.APPROVED_QFORM || x.LoanStatus == ApiConstants.LoanStatus.PENDING).ToList();
                }
                else
                {
                    userLoansFilter = userLoans.Where(x => x.LoanStatus == request.LoanStatus).ToList();
                }


                var lstUserLoanGroupByStatus= userLoansFilter.GroupBy(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
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
                    ListUserLoanRefResponse = lstUserLoanGroupByStatus
                };

                return await Result<SummaryUserLoanRefResponse>.SuccessAsync(summaryUserLoanResponse);
            }
        }
    }
}
