using AutoMapper;
using Firmeza.API.Data.Entities;
using Firmeza.API.DTOs.SaleProduct;

namespace Firmeza.API.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            CreateMap<SaleCreateDto, Sale>()
                .ForMember(dest => dest.SaleProducts,
                    opt => opt.MapFrom(src => src.Products));

            CreateMap<SaleUpdateDto, Sale>()
                .ForMember(dest => dest.SaleProducts,
                    opt => opt.MapFrom(src => src.Products));

            CreateMap<SaleProductDto, SaleProduct>();
            CreateMap<SaleProduct, SaleProductResponseDto>();

            CreateMap<Sale, SaleResponseDto>()
                .ForMember(dest => dest.Products,
                    opt => opt.MapFrom(src => src.SaleProducts));
        }
    }
}
