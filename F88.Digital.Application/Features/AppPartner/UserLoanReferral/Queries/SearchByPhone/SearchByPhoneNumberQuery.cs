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

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries.SearchByPhone
{
    public class SearchByPhoneNumberQuery : IRequest<Result<SummaryUserLoanRefResponse>>
    {
        public int UserProfileId { get; set; }

        public string PhoneNumber { get; set; }

        public class SearchByPhoneNumberQueryHandler : IRequestHandler<SearchByPhoneNumberQuery, Result<SummaryUserLoanRefResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IMapper _mapper;

            public SearchByPhoneNumberQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
            }

            public async Task<Result<SummaryUserLoanRefResponse>> Handle(SearchByPhoneNumberQuery request, CancellationToken cancellationToken)
            {
                var userLoans = await _userLoanReferralRepository.UserLoans
                   .Where(x => x.UserProfileId == request.UserProfileId && x.PhoneNumber.Contains(request.PhoneNumber))
                   .OrderByDescending(x => x.CreatedOn)
                   .Include(x => x.Deposit)
                   .ToListAsync();           

                var lstUserLoanGroupByDate = userLoans.GroupBy(x => x.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
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
