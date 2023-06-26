using F88.Digital.Application.Features.Brands.Commands.Create;
using F88.Digital.Application.Features.Brands.Commands.Update;
using F88.Digital.Application.Features.Brands.Queries.GetAllCached;
using F88.Digital.Application.Features.Brands.Queries.GetById;
using F88.Digital.Web.Areas.Catalog.Models;
using AutoMapper;

namespace F88.Digital.Web.Areas.Catalog.Mappings
{
    internal class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<GetAllBrandsCachedResponse, BrandViewModel>().ReverseMap();
            CreateMap<GetBrandByIdResponse, BrandViewModel>().ReverseMap();
            CreateMap<CreateBrandCommand, BrandViewModel>().ReverseMap();
            CreateMap<UpdateBrandCommand, BrandViewModel>().ReverseMap();
        }
    }
}