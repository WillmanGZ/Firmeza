using AdminManager.Web.Data;
using AdminManager.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace AdminManager.Web.Services
{
    public class ReceiptPdfService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ReceiptPdfService(
            AppDbContext db,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
        }

        public async Task<byte[]> GenerateReceiptPdfAsync(Guid saleId)
        {
            var sale = await _db.Sales
                .Include(s => s.SaleProducts)
                    .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null)
                throw new Exception("Sale not found");

            var client = await _userManager.FindByIdAsync(sale.ClientId);

            var model = new ReceiptPdfModel
            {
                SaleId = sale.Id,
                Date = sale.Date,
                ClientEmail = client?.Email ?? "Unknown",
                ClientPhone = client?.PhoneNumber ?? "",
                Products = sale.SaleProducts.Select(sp => new ReceiptProductItem
                {
                    Name = sp.Product.Name,
                    Quantity = sp.Quantity,
                    UnitPrice = sp.UnitPrice
                }).ToList()
            };

            model.SubTotal = model.Products.Sum(p => p.Total);
            model.VAT = model.SubTotal * 0.19m;
            model.Total = model.SubTotal + model.VAT;

            var pdfDoc = new ReceiptPdfDocument(model);
            var pdfBytes = pdfDoc.GeneratePdf();

            // --- Guardar en wwwroot/recibos ---
            var folder = Path.Combine(_env.WebRootPath, "recibos");
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, $"recibo_{sale.Id}.pdf");
            await File.WriteAllBytesAsync(filePath, pdfBytes);

            return pdfBytes;
        }
    }


}
