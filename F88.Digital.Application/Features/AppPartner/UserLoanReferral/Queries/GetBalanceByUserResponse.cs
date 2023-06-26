using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries
{
    public class GetBalanceByUserResponse
    {
        public int UserProfileId { get; set; }

        public decimal? Balance { get; set; }
    }
}
