using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.ShareService
{
    public class ShareServiceTokenResponse
    {
        public string Data { get; set; }

        public string Error_code { get; set; }

        public string Error_message { get; set; }

        public string Error_detail { get; set; }
    }
}
