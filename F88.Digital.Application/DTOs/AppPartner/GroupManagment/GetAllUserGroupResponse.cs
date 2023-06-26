using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.DTOs.AppPartner.GroupManagment
{
    public class GetAllUserGroupResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserPhone { get; set; }
        public Group Group { get; set; }
    }
}
