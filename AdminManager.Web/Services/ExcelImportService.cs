using AdminManager.Web.Data;
using AdminManager.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace AdminManager.Web.Services
{
    public class ExcelImportService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _users;

        public ExcelImportService(AppDbContext db, UserManager<IdentityUser> users)
        {
            _db = db;
            _users = users;
        }

        public async Task<bool> ImportAsync(Stream stream)
        {
            using var package = new ExcelPackage(stream);

            try
            {
                // PRODUCTS
                var wsProducts = package.Workbook.Worksheets["Products"];
                if (wsProducts != null)
                {
                    for (int row = 2; row <= wsProducts.Dimension.End.Row; row++)
                    {
                        var name = wsProducts.Cells[row, 1].Text.Trim();
                        if (string.IsNullOrWhiteSpace(name)) continue;

                        var p = new Product
                        {
                            Name = name,
                            Description = wsProducts.Cells[row, 2].Text.Trim(),
                            Price = int.Parse(wsProducts.Cells[row, 3].Text.Trim())
                        };

                        _db.Products.Add(p);
                    }

                    await _db.SaveChangesAsync();
                }

                // SALES
                var wsSales = package.Workbook.Worksheets["Sales"];
                List<Sale> importedSales = new();

                if (wsSales != null)
                {
                    for (int row = 2; row <= wsSales.Dimension.End.Row; row++)
                    {
                        var email = wsSales.Cells[row, 2].Text.Trim();
                        if (string.IsNullOrWhiteSpace(email)) continue;

                        var user = await _users.FindByEmailAsync(email);
                        if (user == null) continue;

                        var sale = new Sale
                        {
                            Date = DateTime.Parse(wsSales.Cells[row, 1].Text.Trim()),
                            ClientId = user.Id
                        };

                        importedSales.Add(sale);
                        _db.Sales.Add(sale);
                    }

                    await _db.SaveChangesAsync();
                }

                // SALEPRODUCTS
                var wsSP = package.Workbook.Worksheets["SaleProducts"];
                if (wsSP != null)
                {
                    for (int row = 2; row <= wsSP.Dimension.End.Row; row++)
                    {
                        var saleIndex = int.Parse(wsSP.Cells[row, 1].Text.Trim());
                        var productName = wsSP.Cells[row, 2].Text.Trim();

                        var sale = importedSales.ElementAtOrDefault(saleIndex - 1);
                        if (sale == null) continue;

                        var product = await _db.Products.FirstOrDefaultAsync(p => p.Name == productName);
                        if (product == null) continue;

                        var sp = new SaleProduct
                        {
                            SaleId = sale.Id,
                            ProductId = product.Id,
                            Quantity = int.Parse(wsSP.Cells[row, 3].Text.Trim()),
                            UnitPrice = int.Parse(wsSP.Cells[row, 4].Text.Trim())
                        };

                        _db.SaleProducts.Add(sp);
                    }

                    await _db.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    

    public async Task<byte[]> DownloadTemplateAsync()
        {
            using var package = new ExcelPackage();

            // ----------------------------------------------------
            // 1. PRODUCTS
            // ----------------------------------------------------
            var wsProducts = package.Workbook.Worksheets.Add("Products");

            wsProducts.Cells[1, 1].Value = "Name";
            wsProducts.Cells[1, 2].Value = "Description";
            wsProducts.Cells[1, 3].Value = "Price";

            var products = await _db.Products.ToListAsync();
            int pRow = 2;

            foreach (var p in products)
            {
                wsProducts.Cells[pRow, 1].Value = p.Name;
                wsProducts.Cells[pRow, 2].Value = p.Description;
                wsProducts.Cells[pRow, 3].Value = p.Price;
                pRow++;
            }

            wsProducts.Cells.AutoFitColumns();


            // ----------------------------------------------------
            // 2. SALES
            // ----------------------------------------------------
            var wsSales = package.Workbook.Worksheets.Add("Sales");

            wsSales.Cells[1, 1].Value = "Date";
            wsSales.Cells[1, 2].Value = "ClientEmail";

            var sales = await _db.Sales
                .Include(s => s.Client)
                .ToListAsync();

            int sRow = 2;

            foreach (var s in sales)
            {
                wsSales.Cells[sRow, 1].Value = s.Date.ToString("yyyy-MM-dd");
                wsSales.Cells[sRow, 2].Value = (await _users.FindByIdAsync(s.ClientId))?.Email;
                sRow++;
            }

            wsSales.Cells.AutoFitColumns();


            // ----------------------------------------------------
            // 3. SALE PRODUCTS
            // ----------------------------------------------------
            var wsSP = package.Workbook.Worksheets.Add("SaleProducts");

            wsSP.Cells[1, 1].Value = "SaleId";
            wsSP.Cells[1, 2].Value = "ProductName";
            wsSP.Cells[1, 3].Value = "Quantity";
            wsSP.Cells[1, 4].Value = "UnitPrice";

            var saleProducts = await _db.SaleProducts
                .Include(sp => sp.Product)
                .Include(sp => sp.Sale)
                .ToListAsync();

            int spRow = 2;

            foreach (var sp in saleProducts)
            {
                wsSP.Cells[spRow, 1].Value = sp.SaleId;
                wsSP.Cells[spRow, 2].Value = sp.Product.Name;
                wsSP.Cells[spRow, 3].Value = sp.Quantity;
                wsSP.Cells[spRow, 4].Value = sp.UnitPrice;
                spRow++;
            }

            wsSP.Cells.AutoFitColumns();


            // ----------------------------------------------------
            // 4. CLIENTS
            // ----------------------------------------------------
            var wsClients = package.Workbook.Worksheets.Add("Clients");

            wsClients.Cells[1, 1].Value = "Email";
            wsClients.Cells[1, 2].Value = "PhoneNumber";
            wsClients.Cells[1, 3].Value = "UserId";

            var clients = await _users.GetUsersInRoleAsync("Client");
            int cRow = 2;

            foreach (var c in clients)
            {
                wsClients.Cells[cRow, 1].Value = c.Email;
                wsClients.Cells[cRow, 2].Value = c.PhoneNumber;
                wsClients.Cells[cRow, 3].Value = c.Id;
                cRow++;
            }

            wsClients.Cells.AutoFitColumns();

            // DEVOLVER ARCHIVO
            return package.GetAsByteArray();
        }
    }
}
