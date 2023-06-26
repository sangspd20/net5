using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.Settings
{
    public class OTPSettings
    {
        public string OTPApiUrl { get; set; }

        public string ExpiredOTP { get; set; }

        public string MessageOTP { get; set; }

        public string ReferralMessageOTP { get; set; }
    }
}
