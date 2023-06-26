using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Constants;
using F88.Digital.Application.Extensions;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries
{
    public class GetListUserLoanRefByCurrentMonthQuery : IRequest<Result<SummaryUserLoanRefResponse>>
    {
        public int userProfileId { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; } = ApiConstants.PagingInfo.PAGE_SIZE;

        public class GetListUserLoanRefQueryHandler : IRequestHandler<GetListUserLoanRefByCurrentMonthQuery, Result<SummaryUserLoanRefResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IMapper _mapper;

            public GetListUserLoanRefQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
            }


            public async Task<Result<SummaryUserLoanRefResponse>> Handle(GetListUserLoanRefByCurrentMonthQuery request, CancellationToken cancellationToken)
            {
                var paginatedUserLoans = await _userLoanReferralRepository.UserLoans
                .Where(x => x.CreatedOn.Month == DateTime.Now.Month && x.UserProfileId == request.userProfileId )
                .OrderByDescending(x => x.CreatedOn)
                .Include(x => x.Deposit)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                var lstUserLoanGroupByDate = paginatedUserLoans.Data.GroupBy(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
                                                        .Select(s => new UserLoanRefGroupByDateResponse
                                                        {
                                                            CreatedOn = s.Key.Substring(0, 5),                                                            
                                                            UserLoanRefResponses = _mapper.Map<List<UserLoanRefResponse>>(s.ToList())
                                                        })
                                                        .ToList();
                
                var numberOfPending = paginatedUserLoans.Data.Count(x => x.LoanStatus == ApiConstants.LoanStatus.PENDING || x.LoanStatus == ApiConstants.LoanStatus.APPROVED_QFORM);
                var numberOfApproved = paginatedUserLoans.Data.Count(x => x.LoanStatus == ApiConstants.LoanStatus.APPROVED);
                var numberOfCancel = paginatedUserLoans.Data.Count(x => x.LoanStatus == ApiConstants.LoanStatus.CANCEL);

                var summaryUserLoanResponse = new SummaryUserLoanRefResponse
                {
                    NumberOfPending = numberOfPending,
                    NumberOfApproved = numberOfApproved,
                    NumberOfCancel = numberOfCancel,
                    ListUserLoanRefResponse = lstUserLoanGroupByDate,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                return await Result<SummaryUserLoanRefResponse>.SuccessAsync(summaryUserLoanResponse);
            }
        }
    }
}
