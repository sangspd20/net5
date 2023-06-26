using F88.Digital.Infrastructure.Identity.Models;
using F88.Digital.Web.Areas.Admin.Models;
using AutoMapper;

namespace F88.Digital.Web.Areas.Admin.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}