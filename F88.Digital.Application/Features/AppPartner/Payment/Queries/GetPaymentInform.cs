using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class GetPaymentInform
    {
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }
        public string AccountName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountNumber { get; set; }
        public decimal? NetMoney { get; set; }
        public decimal? CurrentMoney { get; set; }
        public int Tax { get; set; }
        public string MoneyPaid { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public string PaidMonth { get; set; }
        public string PaidYear { get; set; }
        public string Source { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime? TransferDate { get; set; }
        public string OtherAmount { get; set; }
        public bool IsAgreementConfirmed { get; set; }
        public int Status { get; set; }
    }
}
