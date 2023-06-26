using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.Share
{
	public class PawnOnlineAuditSystem
	{
        public int Id { get; set; }

        public string TransactionId { get; set; }

        public string Phone { get; set; }

        public string CustomerName { get; set; }

        public string Asset { get; set; }

        public string DetailStatus { get; set; }

        public string PawnOnlineId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string POLResponse { get; set; }

        public string POLDetailStatus { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string SentPartnerStatus { get; set; }

        public DateTime? SentPartnerDate { get; set; }

        public decimal? LoanValue { get; set; }

        public string PartnerCode { get; set; }

        public string PartnerName { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Address { get; set; }

        public int? CurrentGroupId { get; set; }

        public string Url { get; set; }

        public string Source { get; set; }

        public string AffCreatedStatus { get; set; }

        public string AffiliateFinalStatus { get; set; }

        public string PartnerResponse { get; set; }

        public string SentPartnerRequest { get; set; }

        public string AssetSale { get; set; }

        public bool? IsF88Cus { get; set; }
    }
}
