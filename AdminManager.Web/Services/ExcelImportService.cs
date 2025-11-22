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
        private readonly RoleManager<IdentityRole> _roles;

        public ExcelImportService(AppDbContext db, UserManager<IdentityUser> users, RoleManager<IdentityRole> roles)
        {
            _db = db;
            _users = users;
            _roles = roles;
        }

        public async Task<List<string>> ImportAsync(Stream stream)
        {
            var errors = new List<string>();

            try
            {
                using var package = new ExcelPackage(stream);

                var wsClients = package.Workbook.Worksheets["Clients"];
                var wsProducts = package.Workbook.Worksheets["Products"];
                var wsSales = package.Workbook.Worksheets["Sales"];
                var wsSaleProducts = package.Workbook.Worksheets["SaleProducts"];

                if (wsClients == null) errors.Add("Missing sheet: Clients");
                if (wsProducts == null) errors.Add("Missing sheet: Products");
                if (wsSales == null) errors.Add("Missing sheet: Sales");
                if (wsSaleProducts == null) errors.Add("Missing sheet: SaleProducts");
                if (errors.Any()) return errors;

                // CLIENTS
                var clientsRows = wsClients.Dimension?.End.Row ?? 0;
                for (int row = 2; row <= clientsRows; row++)
                {
                    var idText = wsClients.Cells[row, 1].Text?.Trim();
                    var username = wsClients.Cells[row, 2].Text?.Trim();
                    var email = wsClients.Cells[row, 3].Text?.Trim();
                    var phone = wsClients.Cells[row, 4].Text?.Trim();
                    var password = wsClients.Cells[row, 5].Text?.Trim();

                    if (string.IsNullOrWhiteSpace(idText) ||
                        string.IsNullOrWhiteSpace(username) ||
                        string.IsNullOrWhiteSpace(email) ||
                        string.IsNullOrWhiteSpace(phone) ||
                        string.IsNullOrWhiteSpace(password))
                    {
                        errors.Add($"Clients[{row}]: Required column missing.");
                        continue;
                    }

                    if (!Guid.TryParse(idText, out var clientGuid))
                    {
                        errors.Add($"Clients[{row}]: Invalid Guid '{idText}'.");
                        continue;
                    }

                    var existingById = await _users.FindByIdAsync(clientGuid.ToString());
                    var existingByEmail = await _users.FindByEmailAsync(email);

                    if (existingById != null && existingByEmail != null && existingById.Id != existingByEmail.Id)
                    {
                        errors.Add($"Clients[{row}]: Conflict between Id and Email.");
                        continue;
                    }

                    if (existingById != null)
                    {
                        var changed = false;
                        if (existingById.UserName != username) { existingById.UserName = username; changed = true; }
                        if (existingById.Email != email) { existingById.Email = email; changed = true; }
                        if (existingById.PhoneNumber != phone) { existingById.PhoneNumber = phone; changed = true; }
                        if (changed) await _users.UpdateAsync(existingById);
                        continue;
                    }

                    if (existingByEmail != null)
                    {
                        errors.Add($"Clients[{row}]: Email already exists with different Id.");
                        continue;
                    }

                    var user = new IdentityUser
                    {
                        Id = clientGuid.ToString(),
                        UserName = username,
                        Email = email,
                        PhoneNumber = phone,
                        EmailConfirmed = true
                    };

                    var createResult = await _users.CreateAsync(user, password);
                    if (!createResult.Succeeded)
                    {
                        errors.Add($"Clients[{row}]: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                        continue;
                    }

                    if (!await _roles.RoleExistsAsync("Client"))
                        await _roles.CreateAsync(new IdentityRole("Client"));

                    await _users.AddToRoleAsync(user, "Client");
                }

                // PRODUCTS
                var productsRows = wsProducts.Dimension?.End.Row ?? 0;
                for (int row = 2; row <= productsRows; row++)
                {
                    var idText = wsProducts.Cells[row, 1].Text?.Trim();
                    var name = wsProducts.Cells[row, 2].Text?.Trim();
                    var desc = wsProducts.Cells[row, 3].Text?.Trim();
                    var priceCell = wsProducts.Cells[row, 4].Value;

                    if (string.IsNullOrWhiteSpace(idText) || string.IsNullOrWhiteSpace(name))
                    {
                        errors.Add($"Products[{row}]: Required Id or Name missing.");
                        continue;
                    }

                    if (!Guid.TryParse(idText, out var prodGuid))
                    {
                        errors.Add($"Products[{row}]: Invalid Guid '{idText}'.");
                        continue;
                    }

                    int price;
                    if (priceCell is double dp) price = Convert.ToInt32(dp);
                    else if (!int.TryParse(wsProducts.Cells[row, 4].Text?.Trim(), out price))
                    {
                        errors.Add($"Products[{row}]: Invalid price.");
                        continue;
                    }

                    var existing = await _db.Products.FirstOrDefaultAsync(p => p.Id == prodGuid);
                    if (existing != null)
                    {
                        existing.Name = name;
                        existing.Description = desc ?? "";
                        existing.Price = price;
                        _db.Products.Update(existing);
                    }
                    else
                    {
                        _db.Products.Add(new Product
                        {
                            Id = prodGuid,
                            Name = name,
                            Description = desc ?? "",
                            Price = price
                        });
                    }
                }

                await _db.SaveChangesAsync();

                // SALES 
                var salesRows = wsSales.Dimension?.End.Row ?? 0;
                var salesDict = new Dictionary<Guid, Sale>();

                await using var tx = await _db.Database.BeginTransactionAsync();
                try
                {
                    for (int row = 2; row <= salesRows; row++)
                    {
                        var idText = wsSales.Cells[row, 1].Text?.Trim();
                        var dateCell = wsSales.Cells[row, 2].Value;
                        var clientIdText = wsSales.Cells[row, 3].Text?.Trim();

                        if (!Guid.TryParse(idText, out var saleGuid))
                        {
                            errors.Add($"Sales[{row}]: Invalid Id.");
                            continue;
                        }
                        if (!Guid.TryParse(clientIdText, out var clientGuid))
                        {
                            errors.Add($"Sales[{row}]: Invalid ClientId.");
                            continue;
                        }

                        DateTime date;
                        if (dateCell is double d) date = DateTime.FromOADate(d);
                        else if (!DateTime.TryParse(wsSales.Cells[row, 2].Text?.Trim(), out date))
                        {
                            errors.Add($"Sales[{row}]: Invalid Date.");
                            continue;
                        }

                        // 🔥 CORRECCIÓN IMPORTANTE: PostgreSQL exige UTC
                        date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

                        var client = await _users.FindByIdAsync(clientGuid.ToString());
                        if (client == null)
                        {
                            errors.Add($"Sales[{row}]: Client not found.");
                            continue;
                        }

                        var existingSale = await _db.Sales.FirstOrDefaultAsync(s => s.Id == saleGuid);
                        if (existingSale != null)
                        {
                            salesDict[saleGuid] = existingSale;
                            continue;
                        }

                        var sale = new Sale
                        {
                            Id = saleGuid,
                            Date = date,
                            ClientId = client.Id
                        };

                        _db.Sales.Add(sale);
                        await _db.SaveChangesAsync();

                        salesDict[saleGuid] = sale;
                    }

                    // SALE PRODUCTS
                    var spRows = wsSaleProducts.Dimension?.End.Row ?? 0;
                    for (int row = 2; row <= spRows; row++)
                    {
                        var idText = wsSaleProducts.Cells[row, 1].Text?.Trim();
                        var saleIdText = wsSaleProducts.Cells[row, 2].Text?.Trim();
                        var productIdText = wsSaleProducts.Cells[row, 3].Text?.Trim();
                        var qtyText = wsSaleProducts.Cells[row, 4].Text?.Trim();
                        var unitText = wsSaleProducts.Cells[row, 5].Text?.Trim();

                        if (!Guid.TryParse(idText, out var spGuid)) continue;
                        if (!Guid.TryParse(saleIdText, out var saleGuid)) continue;
                        if (!Guid.TryParse(productIdText, out var productGuid)) continue;

                        if (!int.TryParse(qtyText, out var qty)) continue;
                        if (!int.TryParse(unitText, out var unitPrice)) continue;

                        if (!salesDict.TryGetValue(saleGuid, out var sale))
                        {
                            sale = await _db.Sales.FirstOrDefaultAsync(s => s.Id == saleGuid);
                            if (sale == null)
                            {
                                errors.Add($"SaleProducts[{row}]: Sale not found.");
                                continue;
                            }
                        }

                        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productGuid);
                        if (product == null)
                        {
                            errors.Add($"SaleProducts[{row}]: Product not found.");
                            continue;
                        }

                        if (await _db.SaleProducts.AnyAsync(sp => sp.Id == spGuid))
                        {
                            errors.Add($"SaleProducts[{row}]: Duplicate Id.");
                            continue;
                        }

                        _db.SaleProducts.Add(new SaleProduct
                        {
                            Id = spGuid,
                            SaleId = sale.Id,
                            ProductId = product.Id,
                            Quantity = qty,
                            UnitPrice = unitPrice
                        });
                    }

                    await _db.SaveChangesAsync();
                    await tx.CommitAsync();
                }
                catch (Exception exTx)
                {
                    await _db.Database.RollbackTransactionAsync();
                    errors.Add("Fatal transaction error: " + exTx.Message);
                    return errors;
                }
            }
            catch (Exception ex)
            {
                errors.Add("Fatal: " + ex.Message);
            }

            return errors;
        }



        public async Task<byte[]> DownloadTemplateAsync()
        {
            using var package = new ExcelPackage();

            // 1. PRODUCTS
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


            // 2. SALES
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


            // 3. SALE PRODUCTS
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


            // 4. CLIENTS
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

            return package.GetAsByteArray();
        }
    }
}
