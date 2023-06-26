using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.ShareService
{
    public class GetShopAffiliateResponse
    {
        [JsonProperty("data")]
        public List<DataShopAffiliate> Data { get; set; }
    }
    public class DataShopAffiliate
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
