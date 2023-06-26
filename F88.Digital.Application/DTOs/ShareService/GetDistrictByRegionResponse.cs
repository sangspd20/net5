using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.ShareService
{
    public class GetDistrictByRegionResponse
    {
        public string RegionID { get; set; }

        public string Name { get; set; }

        public int ParentID { get; set; }

        public string ParentName { get; set; }

        public string RegionLevel { get; set; }

        public int Status { get; set; }

        public string AreaID { get; set; }

        public string RegionCode { get; set; }

        public string ProvinceMaplife { get; set; }

        public string ProvinceCodeFinOS { get; set; }
    }
}
