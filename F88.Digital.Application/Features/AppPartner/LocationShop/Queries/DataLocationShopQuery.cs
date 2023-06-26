using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Features.AppPartner.LocationShop.Queries
{
    public class DataLocationShopQuery
    {
        public string Position { get; set; }
        public string ShopName { get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string IsRead { get; set; }
        public string Status { get; set; }
        public string OpenDate { get; set; }
    }

    public class JsonShareService
    {
        public List<DataShareService> data { get; set; }    
    }
    public class DataShareService
    {       
        public string GroupID { get; set; }
        public string status { get; set; }
        public string ShopID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AreaID { get; set; }
        public string Phone { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }

    }
}
