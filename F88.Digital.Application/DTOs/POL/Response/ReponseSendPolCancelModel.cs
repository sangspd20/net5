using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.POL.Response
{
    public class ReponseSendPolCancelModel
    {
        public bool ok { get; set; }
        public int Code { get; set; }
        public ReponseDataSendPolCancelModel data { get; set; }
    }
    public class ReponseDataSendPolCancelModel
    {
        public List<DataRecordset> recordset { get; set; }
    }
    public class DataRecordset
    {
        [JsonProperty(PropertyName = "ID")]
        public string PawnID { get; set; }
        [JsonProperty(PropertyName = "str_city_name")]
        public string Province { get; set; }
        [JsonProperty(PropertyName = "str_district_name")]
        public string District { get; set; }
        [JsonProperty(PropertyName = "city_id")]
        public string ProvinceId { get; set; }
        [JsonProperty(PropertyName = "district_id")]
        public string DistrictId { get; set; }
        [JsonProperty(PropertyName = "str_name")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "str_phone")]
        public string PhoneNumber { get; set; }
        [JsonProperty(PropertyName = "str_phone_etc")]
        public string PhoneNumberEtc { get; set; }
        [JsonProperty(PropertyName = "str_campaign_excel")]
        public string Campaign { get; set; }
        [JsonProperty(PropertyName = "str_url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "str_channel_name")]
        public string SubSource { get; set; }
        [JsonProperty(PropertyName = "str_asset_type_name")]
        public string Asset { get; set; }
    }
}
