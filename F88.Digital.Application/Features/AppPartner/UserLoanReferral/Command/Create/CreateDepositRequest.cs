using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command
{
    public class CreateDepositRequest
    {
        public decimal? BalanceValue { get; set; } = 0;
        public int UserProfileId { get; set; }

        public bool Status { get; set; } = false;

        public string Notes { get; set; }
    }
}
