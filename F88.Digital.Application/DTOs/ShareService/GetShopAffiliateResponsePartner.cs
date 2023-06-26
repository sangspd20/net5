using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.ShareService
{
    public class GetShopAffiliateResponsePartner
    {
        public List<DataShopAffiliatePartner> Data { get; set; }
    }
    public class DataShopAffiliatePartner
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Id { get; set; }
    }
}
