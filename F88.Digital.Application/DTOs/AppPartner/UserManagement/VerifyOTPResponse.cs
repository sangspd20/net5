using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.UserManagement
{
    public class VerifyOTPResponse
    {
        public bool Ok { get; set; }

        public string Message { get; set; }

        public string AccessToken { get; set; }
    }
}
