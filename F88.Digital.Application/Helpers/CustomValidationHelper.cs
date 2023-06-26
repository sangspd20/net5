using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Helpers
{

    public static class CustomValidationHelper
    {

        public static bool IsFormatPhone(string phone)
        {
            int rs = 0;
            var isNumeric = int.TryParse(phone, out rs);
            var valid = true;
            if (phone.Length != 10 || !isNumeric)
            {
                valid = false;
            }
            else
            {
                string[] arr = { "086","096","097","098","032","033","034","035","036","037","038","039", //Viettel
                                 "088","091","094","083","084","085","081","082", //Vina
                                 "089","090","093","070","079","077","076","078", //Mobi
                                 "092","056","058","052", //Vietnammobile
                                 "099","059", //Mobile
                                 "087" //I-Telecom
                };
                var target = phone.Substring(0, 3);
                var results = Array.FindAll(arr, s => s.Equals(target));
                valid = results.Count() > 0 ? true : false;
            }

            return valid;
        }
    }
}
