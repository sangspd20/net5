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

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class GetPaymentDetailsCurrentMonthQuery : IRequest<Result<List<PaymentDetailsResponse>>>
    {
        public int UserProfileId { get; set; }

        public class GetListUserBankQueryHandler : IRequestHandler<GetPaymentDetailsCurrentMonthQuery, Result<List<PaymentDetailsResponse>>>
        {
            private readonly IPaymentRepository _paymentRepository;
            private readonly IMapper _mapper;

            public GetListUserBankQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
            {
                _paymentRepository = paymentRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<PaymentDetailsResponse>>> Handle(GetPaymentDetailsCurrentMonthQuery query, CancellationToken cancellationToken)
            {
                var lstPayments = await _paymentRepository.GetListTransactionPaymentByCurrentMonthsync(query.UserProfileId);
                var mappedlstPayments = new List<PaymentDetailsResponse>();
                var paymentResponse = new PaymentDetailsResponse();

                foreach (var payment in lstPayments)
                {
                    var paymentUserLoan = payment.PaymentUserLoanReferrals.Select(s => s.UserLoanReferral);
                    paymentResponse = _mapper.Map<PaymentDetailsResponse>(payment);
                    paymentResponse.UserLoanReferrals = _mapper.Map<List<UserLoanReferralResponse>>(paymentUserLoan);

                    mappedlstPayments.Add(paymentResponse);
                }

                return Result<List<PaymentDetailsResponse>>.Success(mappedlstPayments);
            }
        }
    }
}
