using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Enums;
using F88.Digital.Application.Helpers;
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
    public class GetFinalBalanceByUserQuery : IRequest<Result<GetBalanceByUserResponse>>
    {
        public int userProfileId { get; set; }

        public class GetFinalBalanceByUserQueryHandler : IRequestHandler<GetFinalBalanceByUserQuery, Result<GetBalanceByUserResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IUserProfileRepository _userProfileRepository;
            private readonly IPaymentRepository _paymentRepository;
            private readonly IMapper _mapper;

            public GetFinalBalanceByUserQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper, IUserProfileRepository userProfileRepository, IPaymentRepository paymentRepository)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
                _userProfileRepository = userProfileRepository;
                _paymentRepository = paymentRepository;
            }

            public async Task<Result<GetBalanceByUserResponse>> Handle(GetFinalBalanceByUserQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    // get user 
                    var user = await _userProfileRepository.GetByUserIdAsync(request.userProfileId);

                    // get total paid for user 
                    var totalPaid = _paymentRepository.GetTotalPaidForUser(user.UserPhone);

                    // get balancer of user
                    var balance = await _userLoanReferralRepository.GetBalanceByUserAsync(request.userProfileId);

                    var result = balance.Balance - totalPaid;

                    GetBalanceByUserResponse response = new GetBalanceByUserResponse()
                    {
                        UserProfileId = request.userProfileId,
                        Balance = result
                    };

                    return await Result<GetBalanceByUserResponse>.SuccessAsync(response);
                }

                catch
                {
                    GetBalanceByUserResponse response = new GetBalanceByUserResponse()
                    {
                        UserProfileId = request.userProfileId,
                        Balance = -1
                    };

                    return await Result<GetBalanceByUserResponse>.SuccessAsync(response);
                }
            }
        }
    }
}
