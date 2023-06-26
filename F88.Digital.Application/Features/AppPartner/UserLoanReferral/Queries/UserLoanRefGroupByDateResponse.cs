using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries
{
    public class UserLoanRefGroupByDateResponse
    {
        public string CreatedOn { get; set; }

        public List<UserLoanRefResponse> UserLoanRefResponses { get; set; }
    }
}
