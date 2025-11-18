namespace Firmeza.API.DTOs.SaleProduct
{
    public class SaleResponseDto
    {
        public Guid Id { get; set; }
        public string ClientId { get; set; }
        public DateTime Date { get; set; }
        public List<SaleProductResponseDto> Products { get; set; } = new();
    }
}
