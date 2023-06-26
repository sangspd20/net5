using AutoMapper;
using F88.Digital.Application.Features.AppPartner.Payment.Command.Create;
using F88.Digital.Application.Features.AppPartner.Payment.Queries;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Mappings.AppPartner
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {      
            CreateMap<CreatePaymentCommand, Payment>();

            CreateMap<Payment, PaymentDetailsResponse>()
                .AfterMap((s, d) => {
                    d.AccNumber = s.UserBank.AccNumber;
                    d.BankName = s.UserBank.Bank.Name;
                });

            CreateMap<UserLoanReferral, UserLoanReferralResponse>();

            CreateMap<Payment, PaymentModel>();
        }
    }
}
