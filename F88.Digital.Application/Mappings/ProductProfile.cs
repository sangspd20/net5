using F88.Digital.Application.Features.Products.Commands.Create;
using F88.Digital.Application.Features.Products.Queries.GetAllCached;
using F88.Digital.Application.Features.Products.Queries.GetAllPaged;
using F88.Digital.Application.Features.Products.Queries.GetById;
using F88.Digital.Domain.Entities.Catalog;
using AutoMapper;

namespace F88.Digital.Application.Mappings
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductCommand, Product>().ReverseMap();
            CreateMap<GetProductByIdResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsCachedResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsResponse, Product>().ReverseMap();
        }
    }
}