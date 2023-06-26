using AutoMapper;
using F88.Digital.Application.DTOs.ShareService;
using F88.Digital.Domain.Entities.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F88.Digital.Application.Mappings.Share
{
    public class ShareObjectProfile : Profile
    {
        public ShareObjectProfile()
        {
            CreateMap<LocationShopResponse, LocationShop>().ReverseMap();
            CreateMap<DataShopAffiliate, DataShopAffiliatePartner>()
                .ForMember(dest => dest.Value , act => act.MapFrom(src => src.Id));
             
        }
    }
}
