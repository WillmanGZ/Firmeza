namespace Firmeza.API.DTOs.SaleProduct
{
    public class SaleProductCreateDto
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }

}
