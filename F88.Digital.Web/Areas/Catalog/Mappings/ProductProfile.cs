using F88.Digital.Application.Features.Products.Commands.Create;
using F88.Digital.Application.Features.Products.Commands.Update;
using F88.Digital.Application.Features.Products.Queries.GetAllCached;
using F88.Digital.Application.Features.Products.Queries.GetById;
using F88.Digital.Web.Areas.Catalog.Models;
using AutoMapper;

namespace F88.Digital.Web.Areas.Catalog.Mappings
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<GetAllProductsCachedResponse, ProductViewModel>().ReverseMap();
            CreateMap<GetProductByIdResponse, ProductViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, ProductViewModel>().ReverseMap();
            CreateMap<UpdateProductCommand, ProductViewModel>().ReverseMap();
        }
    }
}