using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Domain.Entities.Share
{
    public class Region
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public int? RegionLevel { get; set; }
        public int? Status { get; set; }
        public int? AreaId { get; set; }
        public string RegionCode { get; set; }
        public int? ProvinceMaplife { get; set; }
        public int? ProvinceCodeFinOS { get; set; }
    }
}
