using System;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Queries
{
    public class UserLoanRefResponse
    {
        public string PhoneNumber { get; set; }

        public int LoanStatus { get; set; }

        public decimal? RewardMoney { get; set; }

        public string Time
        { 
            get 
            {
                if(CreatedOn != null) return CreatedOn.Value.ToString("HH:mm");

                return string.Empty;
            } 
        }

        public DateTime? CreatedOn { get; set; }
    }
}
