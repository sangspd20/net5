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
    public class GetWaitBalanceByUserQuery : IRequest<Result<GetBalanceByUserResponse>>
    {
        public int userProfileId { get; set; }

        public class GetWaitBalanceByUserQueryHandler : IRequestHandler<GetWaitBalanceByUserQuery, Result<GetBalanceByUserResponse>>
        {
            private readonly IUserLoanReferralRepository _userLoanReferralRepository;
            private readonly IUserProfileRepository _userProfileRepository;
            private readonly IPaymentRepository _paymentRepository;
            private readonly IMapper _mapper;

            public GetWaitBalanceByUserQueryHandler(IUserLoanReferralRepository userLoanReferralRepository, IMapper mapper, IUserProfileRepository userProfileRepository, IPaymentRepository paymentRepository)
            {
                _userLoanReferralRepository = userLoanReferralRepository;
                _mapper = mapper;
                _userProfileRepository = userProfileRepository;
                _paymentRepository = paymentRepository;
            }

            public async Task<Result<GetBalanceByUserResponse>> Handle(GetWaitBalanceByUserQuery request, CancellationToken cancellationToken)
            {
                // get user 
                var user = await _userProfileRepository.GetByUserIdAsync(request.userProfileId);
                // get payment of user in previous month - T-1
                var t1MonthAndYear = CommonHelper.GetPreviousMonthAndYearOfMonthAndYear(DateTime.Now.Month, DateTime.Now.Year);
                var t2viousMonthAndYear = CommonHelper.GetPreviousMonthAndYearOfMonthAndYear(t1MonthAndYear.Month, t1MonthAndYear.Year);
                var paymentsOfUserInPreviousMonth =await _paymentRepository.GetPaymentsOfUserByMonthAsync(user.UserPhone, DateTime.Now.Month, DateTime.Now.Year);
                decimal balanceOfT1Result = 0;
                decimal balanceOfT2Result = 0;
                decimal result = 0;
                bool successOrFailed = false;

                // T-1 have payments data
                if (paymentsOfUserInPreviousMonth.Count() > 0)
                {

                    foreach (var item in paymentsOfUserInPreviousMonth)
                    {

                        if (item.Status == (int)StatusEnum.Failed || item.Status == (int)StatusEnum.Success)
                        {
                            successOrFailed = true;
                            break;
                        }
                    }

                    if(!successOrFailed)
                    {
                        var balanceOfT1ResultObj = _userLoanReferralRepository.GetBalanceByUserInMonth(request.userProfileId, t1MonthAndYear.Month, t1MonthAndYear.Year);
                        if (balanceOfT1ResultObj != null)
                        {
                            balanceOfT1Result = (decimal)balanceOfT1ResultObj.Balance;
                        }
                    }

                } // T-1 not have payments data
                else
                {
                    var balanceOfT1ResultObj = _userLoanReferralRepository.GetBalanceByUserInMonth(request.userProfileId, t1MonthAndYear.Month, t1MonthAndYear.Year);
                    if(balanceOfT1ResultObj != null)
                    {
                        balanceOfT1Result = (decimal)balanceOfT1ResultObj.Balance;
                    }

                    // T-2 payments
                    var paymentsOfUserInT2 = await _paymentRepository.GetPaymentsOfUserByMonthAsync(user.UserPhone, t1MonthAndYear.Month, t1MonthAndYear.Year);
                    if(paymentsOfUserInT2.Count() > 0)
                    {
                        foreach (var item in paymentsOfUserInT2)
                        {
                            if (item.Status == (int)StatusEnum.Pending)
                            {
                                var balanceOfT2ResultObj = _userLoanReferralRepository.GetBalanceByUserInMonth(request.userProfileId, t2viousMonthAndYear.Month, t2viousMonthAndYear.Year);
                                if (balanceOfT2ResultObj != null)
                                {
                                    balanceOfT2Result = (decimal)balanceOfT2ResultObj.Balance;
                                }
                            }
                        }
                    }

                }

                result = successOrFailed ? 0 : balanceOfT1Result + balanceOfT2Result;

                GetBalanceByUserResponse response = new GetBalanceByUserResponse()
                {
                    UserProfileId = request.userProfileId,
                    Balance = result
                };

                return await Result<GetBalanceByUserResponse>.SuccessAsync(response);
            }
        }
    }
}
