using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.UserManagement
{
    public class ReferralUserOtp
    {
        public string UserPhone { get; set; }

        public string OTP { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DeviceId { get; set; }
    }
}
