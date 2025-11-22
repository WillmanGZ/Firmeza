using AdminManager.Web.Data;
using AdminManager.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;


namespace AdminManager.Web.Services
{
    public class AllSalesPdfService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AllSalesPdfService(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<byte[]> GenerateAllSalesPdfAsync()
        {
            var sales = await _db.Sales
                .Include(s => s.SaleProducts)
                    .ThenInclude(sp => sp.Product)
                .ToListAsync();

            var salesData = new List<ReceiptPdfModel>();

            foreach (var sale in sales)
            {
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

                salesData.Add(model);
            }

            var doc = new AllSalesPdfDocument(salesData);
            return doc.GeneratePdf();
        }
    } 

}
