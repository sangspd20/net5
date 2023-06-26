using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.UserManagement
{
   public class CheckOtpShareServiceResponse
    {
        public string data { get; set; }
        public int error_code { get; set; }
        public string error_message { get; set; }
        public string error_detail { get; set; }
    }
}
