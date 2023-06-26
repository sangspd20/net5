using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.Share
{
    public class PawnOnlineSource
    {
        public int Id { get; set; }

        public string GroupSource { get; set; }

        public string SubSource { get; set; }

        public string SubSourceCategory { get; set; }

        public string URLSource { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? CreatedBy { get; set; }

        public bool? Status { get; set; }

        public string Asset { get; set; }

        public string PartnerCode { get; set; }
    }
}
