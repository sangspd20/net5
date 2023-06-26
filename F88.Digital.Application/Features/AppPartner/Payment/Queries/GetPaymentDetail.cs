using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class GetPaymentDetail
    {
        public string TransactionId { get; set; }
        public int UserLoanReferralId { get; set; }
        public string ReferralPhone { get; set; }
        public bool IsF88Cus { get; set; }
        public string PhoneByUser { get; set; }
        public string AssetSale { get; set; }
        public string Asset { get; set; }
        public string FocusTransaction { get; set; }
        public string ContractId { get; set; }
        public string SubSrouce { get; set; }
        public DateTime TransferAppDate { get; set; }
        public DateTime? CreateContractDate { get; set; }
        public DateTime CreateOnlineDate { get; set; }
        public int RefFinalGroupId { get; set; }
        public int RefContractGroupId { get; set; }
        public int RefRealGroupId { get; set; }

        public string PolId { get; set; }
        public decimal? LoanMoney { get; set; }
        public int? Status { get; set; }
        public string PaidMonth { get; set; }
        public string PaidYear { get; set; }
    }
    public class  PawnOnlineAuditSystem
    {
        public string TransactionId { get; set; }
        public string AssetSale { get; set; }
        public string Asset { get; set; }
        public string PawnOnlineId { get; set; }
        public decimal? LoanValue { get; set; }
       
    }
}
