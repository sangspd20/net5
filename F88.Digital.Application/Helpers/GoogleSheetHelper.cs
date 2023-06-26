using F88.Digital.Application.Generics;
using F88.Digital.Application.Objects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace F88.Digital.Application.Helpers
{
    public static class GoogleSheetHelper
    {
        static SheetsService service;
        public static SheetsService Authentication(string[] scopes,string applicationName)
        {
            GoogleCredential credential;
            //Reading Credentials File...
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(scopes);
            }
            // Creating Google Sheets API service...
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
            return service;
        }
        public static void UpdateData(string column, int position, string value, string spreadsheetId, string sheetName)
        {
            var range = $"{sheetName}!{column}{position}";
            var valueRange = new ValueRange();

            // Setting Cell Value...
            var oblist = new List<object>() { value };
            valueRange.Values = new List<IList<object>> { oblist };
            // Performing Update Operation...
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = updateRequest.Execute();
        }
        public static string UpdateProvince(string province)
        {
            var result = "";
            if (province.Equals("Đăk Lăk"))
            {
                result = "Đắk Lắk";
            }
            else if (province.Equals("Đăk Mil"))
            {
                result = "Đắk Mil";
            }
            else if (province.Equals("Đăk Nông"))
            {
                result = "Đắk Nông";
            }
            else
            {
                result = province;
            }
            return result;
        }
    }
    public static class ColumnGoogleSheet
    {
        public static string I = "I";
        public static string J = "J";
        public static string F = "F";
        public static string K = "K";
        public static string H = "H";
        public static string G = "G";
        public static string L = "L";
        public static string M = "M";
        public static string O = "O";
    }
   
}
