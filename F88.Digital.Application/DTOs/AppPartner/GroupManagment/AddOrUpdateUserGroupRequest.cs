using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.GroupManagment
{
    public class AddOrUpdateUserGroupRequest
    {
        public List<int> ListUserProfileId { get; set; }
        public int GroupId { get; set; }
        public string CreatedBy { get; set; }
    }
}
