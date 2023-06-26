using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class GetPaymentHistory
    {
        public string TransferDate { get; set; }
        public List<PaymentModel> Payment { get; set; }
    }   
    public class PaymentModel
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public decimal? PaidValue { get; set; }

        public decimal? TaxValue { get; set; }

        public decimal? OtherAmount { get; set; }

        public DateTime? TransferDate { get; set; }

        public int Status { get; set; }

        public string Notes { get; set; }

        public string UserPhone { get; set; }

        public int UserBankId { get; set; }
    
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PaidMonth { get; set; }
        public string PaidYear { get; set; }

    }
}
