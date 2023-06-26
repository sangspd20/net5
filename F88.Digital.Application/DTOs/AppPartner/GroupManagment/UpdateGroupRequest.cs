using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.GroupManagment
{
   public class UpdateGroupRequest
    {
        public int groupId { get; set; }
        public string Name { get; set; }
    }
}
