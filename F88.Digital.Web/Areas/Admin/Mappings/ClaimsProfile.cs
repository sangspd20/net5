using F88.Digital.Web.Areas.Admin.Models;
using AutoMapper;
using System.Security.Claims;

namespace F88.Digital.Web.Areas.Admin.Mappings
{
    public class ClaimsProfile : Profile
    {
        public ClaimsProfile()
        {
            CreateMap<Claim, RoleClaimsViewModel>().ReverseMap();
        }
    }
}