using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Queries
{
    public class UserNotificationResponse
    {
        public int UserProfileId { get; set; }

        public string NotiTypeCode { get; set; }

        public string NotiTitle { get; set; }

        public string NotiDetail { get; set; }

        public DateTime? NotiStartDate { get; set; }

        //public DateTime? NotiEndDate { get; set; }

        public string NotiIconUrl { get; set; }

        public string Date
        {
            get
            {
                return NotiStartDate.HasValue ? NotiStartDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            }
        }

        public string Time
        {
            get
            {
                return NotiStartDate.HasValue ? NotiStartDate.Value.ToString("HH:mm") : string.Empty;
            }
        }

        public int NotiType { get; set; }
        public int PaymentId { get; set; }

        public bool IsRead { get; set; }
    }
}
