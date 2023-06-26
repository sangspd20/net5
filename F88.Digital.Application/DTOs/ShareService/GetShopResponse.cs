using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.ShareService
{

    /// <summary>
    /// POS model
    /// </summary>
    public class GetShopResponse
    {
        public int GroupID { get; set; }

        public int Status { get; set; }

        public int ShopID { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int AreaID { get; set; }

        public string Phone { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string BankBranch { get; set; }
    }

    /// <summary>
    /// Affiliate Model
    /// </summary>
    public class LocationShopResponse
    {
        public int ID { get; set; }
        public string Province { get; set; }
        public string County { get; set; }
        public string Shop { get; set; }
        public string RegionID { get; set; }
        public int GroupID { get; set; }
        public bool? Status { get; set; }
    }
}
