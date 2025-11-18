namespace Firmeza.API.DTOs.SaleProduct
{
    public class SaleCreateDto
    {
        public string ClientId { get; set; }
        public List<SaleProductDto> Products { get; set; } = new();
    }
}
