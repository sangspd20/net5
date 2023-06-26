using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner
{
    public class UserOTPRequest 
    {
        public string UserPhone { get; set; }

        public string OTP { get; set; }

        public string DeviceId { get; set; }
        public string Type { get; set; }
    }
}
