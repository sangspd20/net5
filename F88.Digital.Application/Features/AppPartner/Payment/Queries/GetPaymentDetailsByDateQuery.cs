using AspNetCoreHero.Results;
using AutoMapper;
using F88.Digital.Application.Interfaces.Repositories.AppPartner;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class GetPaymentDetailsByDateQuery : IRequest<Result<List<PaymentDetailsResponse>>>
    {
        public int userProfileId { get; set; }

        [Column(TypeName = "Date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ToDate { get; set; }

        public class GetPaymentDetailsByDateQueryHandler : IRequestHandler<GetPaymentDetailsByDateQuery, Result<List<PaymentDetailsResponse>>>
        {
            private readonly IPaymentRepository _paymentRepository;
            private readonly IMapper _mapper;

            public GetPaymentDetailsByDateQueryHandler(IPaymentRepository paymentRepository, IMapper mapper)
            {
                _paymentRepository = paymentRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<PaymentDetailsResponse>>> Handle(GetPaymentDetailsByDateQuery query, CancellationToken cancellationToken)
            {
                var lstPayments = await _paymentRepository.GetListTransactionPaymentByDateAsync(query.userProfileId, query.FromDate, query.ToDate);
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
