using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
  public class PaymentUserLoanResponse
  {
        public int UserLoanReferralId { get; set; }

        public UserLoanReferralResponse UserLoanReferral { get; set; }
  }
}
