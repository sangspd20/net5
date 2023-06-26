using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.Payment.Queries
{
    public class UserLoanReferralResponse
    {
        public string PhoneNumber { get; set; }

        public string Time
        {
            get
            {
                if (CreatedOn != null) return CreatedOn.Value.ToString("HH:mm");

                return string.Empty;
            }
        }

        public string Date
        {
            get
            {
                if (CreatedOn != null) return CreatedOn.Value.ToString("dd/MM");

                return string.Empty;
            }
        }

        public DateTime? CreatedOn { get; set; }
    }
}
