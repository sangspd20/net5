using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries
{
    public class SummaryUserLoanRefResponse
    {
        public int NumberOfPending { get; set; }

        public int NumberOfApproved { get; set; }

        public int NumberOfCancel { get; set; }

        public List<UserLoanRefGroupByDateResponse> ListUserLoanRefResponse { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalCount 
        { 
            get 
            {
                if (ListUserLoanRefResponse == null) return 0;

                return ListUserLoanRefResponse.Count;
            } 
        }
    }
}
