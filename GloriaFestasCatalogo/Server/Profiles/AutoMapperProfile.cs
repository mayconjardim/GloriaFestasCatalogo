using AutoMapper;
using GloriaFestasCatalogo.Shared.Dtos.Config;
using GloriaFestasCatalogo.Shared.Dtos.Orders;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Models.Config;
using GloriaFestasCatalogo.Shared.Models.Orders;
using GloriaFestasCatalogo.Shared.Models.Products;

namespace GloriaFestasCatalogo.Server.Profiles
{
	public class AutoMapperProfile : Profile
	{

		public AutoMapperProfile()
		{


			CreateMap<Product, ProductDto>()
				.ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants));

			CreateMap<ProductVariant, ProductVariantDto>()
				.ForMember(dest => dest.ProductTypeName, opt => opt.MapFrom(src => src.ProductType.Name));

			CreateMap<ProductDto, Product>();

			CreateMap<ProductVariantDto, ProductVariant>();

			CreateMap<ProductCategoryDto, ProductCategory>().ReverseMap();

			CreateMap<ProductCreateDto, Product>()
				.ForMember(dest => dest.Categories, opt => opt.Ignore())
				.ForMember(dest => dest.Variants, opt => opt.Ignore());


			CreateMap<OrderDto, Order>().ReverseMap();

			CreateMap<OrderCreateDto, Order>()
				.ForMember(dest => dest.Products, opt => opt.Ignore());

			CreateMap<Order, OrderCreateDto>();

			CreateMap<OrderCart, OrderCartDto>();

			CreateMap<AppConfig, AppConfigDto>().ReverseMap();

			CreateMap<ProductType, ProductTypeDto>().ReverseMap();




		}
	}
}
