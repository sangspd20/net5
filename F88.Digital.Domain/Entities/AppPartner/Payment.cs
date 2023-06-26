using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.AppPartner
{
    public class Payment : AuditableEntity
    {

        public Payment()
        {
            PaymentUserLoanReferrals = new List<PaymentUserLoanReferral>();
        }

        public decimal? PaidValue { get; set; }

        public decimal? TaxValue { get; set; }

        public decimal? OtherAmount { get; set; }

        public DateTime? TransferDate { get; set; }

        /// <summary>
        /// Thành công: 1
        /// Thất bại: -1
        /// pending : 0
        /// </summary>
        public int Status { get; set; }

        public string Notes { get; set; }

        public string UserPhone  { get; set; }
        public string PaidMonth { get; set; }
        public string PaidYear { get; set; }

        public int UserBankId { get; set; }

        public UserBank UserBank { get; set; }

        #region Extension
        public virtual List<PaymentUserLoanReferral> PaymentUserLoanReferrals { get; set; }
        #endregion

    }
}


