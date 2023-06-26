using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.NotiApplication.Queries
{
   public class AppNotificationResponse
    {
        public int NotiType { get; set; }

        public string NotiTitle { get; set; }

        public string NotiDetail { get; set; }

        public string NotiIconUrl { get; set; }

        public DateTime NotiDate { get; set; }

        public string Date
        {
            get
            {
                return NotiDate.ToString("dd/MM/yyyy");
            }
        }

        public string Time
        {
            get
            {
                return NotiDate.ToString("HH:mm");
            }
        }
    }
}
