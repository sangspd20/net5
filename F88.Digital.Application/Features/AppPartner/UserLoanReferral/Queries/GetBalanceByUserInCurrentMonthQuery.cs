using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries
{
    public class GetBalanceByUserInCurrentMonthQuery : IRequest<Result<GetBalanceByUserResponse>>
    {
        public int userProfileId { get; set; }

        public class GetBalanceByUserInCurrentMonthQueryHandler : IRequestHandler<GetBalanceByUserInCurrentMonthQuery, Result<GetBalanceByUserResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IMapper _mapper;

            public GetBalanceByUserInCurrentMonthQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
            }

            public async Task<Result<GetBalanceByUserResponse>> Handle(GetBalanceByUserInCurrentMonthQuery request, CancellationToken cancellationToken)
            {
                var balanceByUserResponse = await _userLoanReferralRepository.GetBalanceByUserInCurrentMonthAsync(request.userProfileId);
                return await Result<GetBalanceByUserResponse>.SuccessAsync(balanceByUserResponse);
            }
        }
    }
}
