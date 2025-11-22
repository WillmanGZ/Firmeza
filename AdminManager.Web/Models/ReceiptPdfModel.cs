namespace AdminManager.Web.Models
{
    public class ReceiptPdfModel
    {
        public Guid SaleId { get; set; }
        public DateTime Date { get; set; }

        public string ClientEmail { get; set; } = "";
        public string ClientPhone { get; set; } = "";

        public List<ReceiptProductItem> Products { get; set; } = new();

        public decimal SubTotal { get; set; }
        public decimal VAT { get; set; }
        public decimal Total { get; set; }
    }

    public class ReceiptProductItem
    {
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

        public decimal Total => Quantity * UnitPrice;
    }


}
