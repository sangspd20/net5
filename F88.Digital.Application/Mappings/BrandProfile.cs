using F88.Digital.Application.Features.Brands.Commands.Create;
using F88.Digital.Application.Features.Brands.Queries.GetAllCached;
using F88.Digital.Application.Features.Brands.Queries.GetById;
using F88.Digital.Domain.Entities.Catalog;
using AutoMapper;

namespace F88.Digital.Application.Mappings
{
    internal class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<CreateBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsCachedResponse, Brand>().ReverseMap();
        }
    }
}