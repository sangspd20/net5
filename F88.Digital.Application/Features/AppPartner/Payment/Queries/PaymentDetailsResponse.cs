using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class PaymentDetailsResponse
    {
        public PaymentDetailsResponse()
        {
            UserLoanReferrals = new List<UserLoanReferralResponse>();
        }
        public int Id { get; set; }

        public decimal? PaidValue { get; set; }

        public decimal? TaxValue { get; set; }

        public decimal? OtherAmount { get; set; }

        public DateTime? TransferDate { get; set; }

        public string AccNumber { get; set; }

        public string BankName { get; set; }

        /// <summary>
        /// Thành công: 1
        /// Thất bại: 0
        /// </summary>
        public bool Status { get; set; }

        public string Notes { get; set; }

        public List<UserLoanReferralResponse> UserLoanReferrals { get; set; }
    }
}
