using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.ShareService
{
    public class CheckDupPhoneResponse
    {
        public bool ok { get; set; }

        public string phone_number { get; set; }
    }
}
