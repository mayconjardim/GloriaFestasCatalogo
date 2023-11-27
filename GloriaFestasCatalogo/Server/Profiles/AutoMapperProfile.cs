using AutoMapper;
using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Orders;
using GloriaFestasCatalogo.Shared.Models.Products;

namespace GloriaFestasCatalogo.Server.Profiles
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<ProductDto, Product>().ReverseMap();

            CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();

            CreateMap<OrderDto, Order>().ReverseMap();

        }
    }
}
