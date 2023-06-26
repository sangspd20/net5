using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.UserManagement
{
    public class CheckOtpShareServiceRequest
    {
        public string refId { get; set; }
        public string transactionType
        {
            get
            {
                return "GROWTH_PARTNER";
            }
        }
        public string otp { get; set; }
    }
}
