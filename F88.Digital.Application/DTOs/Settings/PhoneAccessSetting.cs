using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.Settings
{
    public class PhoneAccessSetting
    {
        public string Phone { get; set; }   
    }

    public class SpreadSheetIdSetting
    {
        public string FacebookSpreadSheetId { get; set; }
        public string FacebookMessSpreadSheetId { get; set; }
        public string GoogleSpreadSheetId { get; set; }
        public string Google2ndSpreadSheetId { get; set; }
        public string GoogleOnwedSpreadSheetId { get; set; }
        public string PartnershipSpreadSheetId { get; set; }        
    }

    public class UrlPOLSetting
    {
        public string UrlPOL { get; set; }
        public string UrlPOLCancel { get; set; }
        public string PageSize { get; set; }
    }
    public class UrlShareServiceSetting
    {
        public string UrlShareService { get; set; }
    }
}
