using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.UserLoanReferral.Command.Create
{
    public class CreateAffiliateResultResponse
    {
        public string TransactionId { get; set; } //Mã đối soát
        public bool IsF88Cus { get; set; } // là KH F88    
        [JsonProperty("GroupId")]
        public string RefRealGroupId { get; set; }
        [JsonProperty("PolId")]
        public string PolId { get; set; }
    }
}
