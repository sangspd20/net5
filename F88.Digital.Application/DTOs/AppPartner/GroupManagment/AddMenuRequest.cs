using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.GroupManagment
{
    public class AddMenuRequest
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public string CreatedBy { get; set; }
    }
}
