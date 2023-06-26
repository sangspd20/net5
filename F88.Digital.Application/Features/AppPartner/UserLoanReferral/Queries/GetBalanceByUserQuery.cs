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
    public class GetBalanceByUserQuery : IRequest<Result<GetBalanceByUserResponse>>
    {
        public int userProfileId { get; set; }

        public class GetBalanceByUserQueryHandler : IRequestHandler<GetBalanceByUserQuery, Result<GetBalanceByUserResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IMapper _mapper;

            public GetBalanceByUserQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
            }

            public async Task<Result<GetBalanceByUserResponse>> Handle(GetBalanceByUserQuery request, CancellationToken cancellationToken)
            {
                var balanceByUserResponse = await _userLoanReferralRepository.GetBalanceByUserAsync(request.userProfileId);
                return await Result<GetBalanceByUserResponse>.SuccessAsync(balanceByUserResponse);
            }
        }
    }
}
