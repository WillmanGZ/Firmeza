namespace Firmeza.API.DTOs.Sale
{
    public class SaleFullProductDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}
