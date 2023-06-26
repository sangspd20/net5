using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.GroupManagment
{
    public class GetGroupMenuResponse
    {
        public List<Group> Groups { get; set; }
        public List<Menu> Menus { get; set; }
    }
}
