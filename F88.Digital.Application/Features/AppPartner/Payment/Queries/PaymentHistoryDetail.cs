using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class PaymentHistoryDetail
    {
        public string PhoneNumber { get; set; }
        public decimal? CurrentMoney { get; set; }
        public string TransferDate { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode{ get; set; }
        public decimal? TaxValue { get; set; }
        public decimal? OtherAmount { get; set; }
        public decimal? NetMoney { get; set; }
        public int Status { get; set; }
        public string Notes { get; set; }
    }
}
