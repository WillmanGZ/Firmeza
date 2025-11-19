using Firmeza.API.DTOs.SaleProduct;

namespace Firmeza.API.DTOs.Sale
{
    public class SaleResponseDto
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public DateTime Date { get; set; }
        public List<SaleProductResponseDto> Products { get; set; } = new();
    }
}
