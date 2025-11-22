using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AdminManager.Web.Models
{
    public class AllSalesPdfDocument : IDocument
    {
        private readonly List<ReceiptPdfModel> Sales;

        public AllSalesPdfDocument(List<ReceiptPdfModel> sales)
        {
            Sales = sales;
        }

        public DocumentMetadata GetMetadata()
            => new DocumentMetadata { Title = "All Sales Report" };

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Content().Column(col =>
                {
                    foreach (var sale in Sales)
                    {
                        col.Item().Element(e => ComposeSale(e, sale));

                        // Separador entre ventas
                        col.Item().PaddingVertical(10)
                            .LineHorizontal(1).LineColor("#CCC");
                    }
                });
            });
        }

        private void ComposeSale(IContainer container, ReceiptPdfModel model)
        {
            container.Column(col =>
            {
                col.Item().Text($"Sale #{model.SaleId}").FontSize(16).SemiBold();
                col.Item().Text($"Date: {model.Date:yyyy-MM-dd}");
                col.Item().Text($"Client: {model.ClientEmail}");

                col.Item().PaddingVertical(10).LineHorizontal(1);

                // Tabla de productos
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(3);
                        c.RelativeColumn(1);
                        c.RelativeColumn(1);
                        c.RelativeColumn(1);
                    });

                    table.Header(h =>
                    {
                        h.Cell().Element(H).Text("Product");
                        h.Cell().Element(H).Text("Qty");
                        h.Cell().Element(H).Text("Unit");
                        h.Cell().Element(H).Text("Total");
                    });

                    static IContainer H(IContainer c) =>
                        c.Background("#EEE").Padding(5);

                    foreach (var p in model.Products)
                    {
                        table.Cell().Padding(5).Text(p.Name);
                        table.Cell().Padding(5).Text(p.Quantity);
                        table.Cell().Padding(5).Text($"{p.UnitPrice:C}");
                        table.Cell().Padding(5).Text($"{p.Total:C}");
                    }
                });

                col.Item().PaddingTop(10).AlignRight().Column(totals =>
                {
                    totals.Item().Text($"Subtotal: {model.SubTotal:C}");
                    totals.Item().Text($"VAT (19%): {model.VAT:C}");
                    totals.Item().Text($"Total: {model.Total:C}").SemiBold();
                });
            });
        }
    }
}
