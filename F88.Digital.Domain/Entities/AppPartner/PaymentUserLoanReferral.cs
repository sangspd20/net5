using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
   public class PaymentUserLoanReferral : AuditableEntity
    {
        public int PaymentId { get; set; }

        public virtual Payment Payment { get; set; }

        public int UserLoanReferralId { get; set; }
        public string TransactionId { get; set; }
        public int Status { get; set; }

        public virtual UserLoanReferral UserLoanReferral { get; set; }
    }
}
