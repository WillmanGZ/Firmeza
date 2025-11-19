using AutoMapper;
using Firmeza.API.Data.Entities;
using Firmeza.API.DTOs.SaleProduct;

public class SaleProductProfile : Profile
{
    public SaleProductProfile()
    {
        CreateMap<SaleProductCreateDto, SaleProduct>();
        CreateMap<SaleProductUpdateDto, SaleProduct>();
        CreateMap<SaleProduct, SaleProductResponseDto>();
    }
}

