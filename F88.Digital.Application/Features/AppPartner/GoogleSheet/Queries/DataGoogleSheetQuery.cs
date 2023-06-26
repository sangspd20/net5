using System;
using System.Collections.Generic;

namespace F88.Digital.Application.Features.AppPartner.GoogleSheet.Queries
{
    public class DataGoogleSheetQuery
    {
        public string Position { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string GroupId { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string IsRead { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Records { get; set; }
        public string Asset { get; set; }
        public string PolId { get; set; }
    }
    public class RequestSendPolQuery
    {
        public string PawnID { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public bool IsCancel  { get; set; }
        public string Asset { get; set; }
        public string SubSource { get; set; }
        public string RegionID { get; set; }
    }
    public class GroupProvinceQuery
    {
        public int GroupId { get; set; }
       
    }
    public class JsonReponseQuery
    {
        public List<string> data { get; set; }
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
        public string systemMessage { get; set; }

    }
}
