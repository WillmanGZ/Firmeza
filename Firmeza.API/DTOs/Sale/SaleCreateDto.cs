using Firmeza.API.DTOs.SaleProduct;

namespace Firmeza.API.DTOs.Sale
{
    public class SaleCreateDto
    {
        public string ClientId { get; set; }
        public List<SaleProductResponseDto> Products { get; set; } = new();
    }
}
