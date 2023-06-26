using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.ShareService
{
    public class GetRegionLevelResponse
    {
        public int id { get; set; }

        public int region_id { get; set; }

        public string name { get; set; }

        public int? parent_id { get; set; }

        public string parent_name { get; set; }

        public int region_level { get; set; }

        public int state { get; set; }

        public int area_id { get; set; }
    }
}
