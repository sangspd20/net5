using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.Settings
{
    public class ApiShareServiceSettings
    {
        public string LoginApiUrl { get; set; }

        public string GetRegionLevelApiUrl { get; set; }

        public string GetDistrictApiUrl { get; set; }

        public string GetAllShopApiUrl { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string POLCheckDupPhoneApiUrl { get; set; }

        public int POLExistDay { get; set; }
    }
}
