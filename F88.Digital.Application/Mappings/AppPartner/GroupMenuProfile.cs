using AutoMapper;
using F88.Digital.Application.DTOs.AppPartner.GroupManagment;
using F88.Digital.Domain.Entities.AppPartner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Mappings.AppPartner
{
    internal class GroupMenuProfile : Profile
    {
        public GroupMenuProfile()
        {
            CreateMap<AddGroupRequest, Group>().ReverseMap();
            CreateMap<AddMenuRequest, Menu>().ReverseMap();
            CreateMap<AddOrUpdateGroupMenuRequest, GroupMenu>().ReverseMap();
            CreateMap<AddOrUpdateUserGroupRequest, UserGroup>().ReverseMap();
        }
    }
}
