using AutoMapper;
using Firmeza.API.Data.Entities;
using Firmeza.API.DTOs.Product;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductResponseDto>();
        CreateMap<ProductCreateDto, Product>();
        CreateMap<ProductUpdateDto, Product>();
    }
}
