using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.Share
{
	public class LocationShop
	{
		public int ID { get; set; }
		public string Province { get; set; }
		public string Province_Eng { get; set; }
		public string County { get; set; }
		public string Shop { get; set; }
		public string RegionID { get; set; }
		public int? GroupID { get; set; }
		public int? GroupIdOld { get; set; }		
		public int? IsGet { get; set; }
		public string Camp { get; set; }
		public DateTime? Updated { get; set; }
		public bool? Status { get; set; }
		public string PartnerApplyTransfer { get; set; }
	}
}
