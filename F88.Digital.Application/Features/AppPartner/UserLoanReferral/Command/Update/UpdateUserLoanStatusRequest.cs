using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command.Update
{
    public class UpdateUserLoanStatusRequest
    {
        public string TransactionId { get; set; }

        public int LoanStatus { get; set; }

        public string Notes { get; set; }

        public int RefContractGroupId { get; set; }
        public decimal LoanAmount { get; set; }
        public string PawnId { get; set; }
        public string AssetType { get; set; }
    }
}

