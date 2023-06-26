using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UploadFile
{
    public class InformAppPartner
    {
        public List<PayInform> PayInform { get; set; }
        public List<PayDetail> PayDetail { get; set; }     
    }

    public class PayInform
    {
        public string PhoneNumber { get; set; }
        public string IdCard { get; set; }
        public string CollaboratorName { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string AccountNumber { get; set; }
        public string NetMoney { get; set; }
        public string CurrentMoney { get; set; }
        public string Tax { get; set; }
        public string MoneyPaid { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public string PayDate { get; set; }
        public string OtherAmount { get; set; }
        public string AcceptPrivacy { get; set; }
        public string Status { get; set; }
        public string PaidMonth { get; set; }
        public string PaidYear { get; set; }
    }
    public class PayDetail
    {
        public string TransactionId { get; set; }
        public string UserLoanReferralId{ get; set; }
        public string ReferralPhone { get; set; }
        public string IsF88Cus { get; set; }
        public string PhoneByUser { get; set; }
        public string TSContract { get; set; }
        public string LoanApplication { get; set; }
        public string FocusTransaction { get; set; }
        public string ContractCode { get; set; }
        public string SubSrouce { get; set; }
        public string TransferAppDate { get; set; }
        public string CreateContractDate { get; set; }
        public string CreateOnlineDate { get; set; }
        public string TransactionLastReceive { get; set; }
        public string TransactionCreate { get; set; }
        public string TransactionReceive { get; set; }
        public string PawnOnlineWid { get; set; }
        public string LoanMoney { get; set; }
        public string Status { get; set; }
        public string PaidMonth { get; set; }
        public string PaidYear { get; set; }
    }
}
