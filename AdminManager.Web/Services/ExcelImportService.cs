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

                var salesDict = new Dictionary<string, Sale>();

                await using var trx = await _db.Database.BeginTransactionAsync();

                // -------------------------------------------------------
                // 1) CLIENTES
                // -------------------------------------------------------
                if (wsClients != null && wsClients.Dimension != null)
                {
                    for (int row = 2; row <= wsClients.Dimension.End.Row; row++)
                    {
                        var email = wsClients.Cells[row, 1].Text?.Trim();
                        var username = wsClients.Cells[row, 2].Text?.Trim();
                        var phone = wsClients.Cells[row, 3].Text?.Trim();
                        var password = wsClients.Cells[row, 4].Text?.Trim();

                        if (string.IsNullOrWhiteSpace(email))
                            continue;

                        var existing = await _users.FindByEmailAsync(email);
                        if (existing != null)
                            continue;

                        var user = new IdentityUser
                        {
                            Email = email,
                            UserName = string.IsNullOrWhiteSpace(username) ? email : username,
                            PhoneNumber = phone
                        };

                        var pwd = string.IsNullOrWhiteSpace(password) ? "User123*" : password;

                        var result = await _users.CreateAsync(user, pwd);
                        if (!result.Succeeded)
                        {
                            errors.Add($"Error creando usuario {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                            continue;
                        }

                        if (!await _roles.RoleExistsAsync("Client"))
                            await _roles.CreateAsync(new IdentityRole("Client"));

                        await _users.AddToRoleAsync(user, "Client");
                    }
                }

                // -------------------------------------------------------
                // 2) PRODUCTOS
                // -------------------------------------------------------
                if (wsProducts != null && wsProducts.Dimension != null)
                {
                    for (int row = 2; row <= wsProducts.Dimension.End.Row; row++)
                    {
                        var name = wsProducts.Cells[row, 1].Text?.Trim();
                        if (string.IsNullOrWhiteSpace(name)) continue;

                        var desc = wsProducts.Cells[row, 2].Text?.Trim();
                        var priceText = wsProducts.Cells[row, 3].Text?.Trim();

                        if (!int.TryParse(priceText, out int price))
                        {
                            errors.Add($"Fila {row} en Products tiene Price inválido.");
                            continue;
                        }

                        var exists = await _db.Products.FirstOrDefaultAsync(p => p.Name == name);
                        if (exists != null)
                            continue;

                        _db.Products.Add(new Product
                        {
                            Name = name,
                            Description = desc ?? "",
                            Price = price
                        });
                    }

                    await _db.SaveChangesAsync();
                }

                // -------------------------------------------------------
                // 3) SALES
                // Key: date|email
                // -------------------------------------------------------
                if (wsSales != null && wsSales.Dimension != null)
                {
                    for (int row = 2; row <= wsSales.Dimension.End.Row; row++)
                    {
                        DateTime date;
                        var rawDate = wsSales.Cells[row, 1].Value;

                        if (rawDate is double d)
                            date = DateTime.FromOADate(d);
                        else if (!DateTime.TryParse(wsSales.Cells[row, 1].Text?.Trim(), out date))
                        {
                            errors.Add($"Fecha invalida en Sales fila {row}");
                            continue;
                        }

                        var email = wsSales.Cells[row, 2].Text?.Trim();
                        if (string.IsNullOrWhiteSpace(email)) continue;

                        var client = await _users.FindByEmailAsync(email);
                        if (client == null)
                        {
                            errors.Add($"Cliente no encontrado en Sales fila {row}: {email}");
                            continue;
                        }

                        var key = $"{date:yyyy-MM-dd}|{email.ToLower()}";

                        if (!salesDict.ContainsKey(key))
                        {
                            var sale = new Sale
                            {
                                Date = date,
                                ClientId = client.Id
                            };

                            _db.Sales.Add(sale);
                            await _db.SaveChangesAsync();

                            salesDict[key] = sale;
                        }
                    }
                }

                // -------------------------------------------------------
                // 4) SALEPRODUCTS
                // -------------------------------------------------------
                if (wsSaleProducts != null && wsSaleProducts.Dimension != null)
                {
                    for (int row = 2; row <= wsSaleProducts.Dimension.End.Row; row++)
                    {
                        DateTime date;

                        var rawDate = wsSaleProducts.Cells[row, 1].Value;
                        if (rawDate is double d)
                            date = DateTime.FromOADate(d);
                        else if (!DateTime.TryParse(wsSaleProducts.Cells[row, 1].Text?.Trim(), out date))
                        {
                            errors.Add($"Fecha inválida en SaleProducts fila {row}");
                            continue;
                        }

                        var email = wsSaleProducts.Cells[row, 2].Text?.Trim();
                        var productName = wsSaleProducts.Cells[row, 3].Text?.Trim();
                        var qtyText = wsSaleProducts.Cells[row, 4].Text?.Trim();
                        var unitText = wsSaleProducts.Cells[row, 5].Text?.Trim();

                        if (!int.TryParse(qtyText, out int qty))
                        {
                            errors.Add($"Cantidad inválida en SaleProducts fila {row}");
                            continue;
                        }
                        if (!int.TryParse(unitText, out int unitPrice))
                        {
                            errors.Add($"UnitPrice inválido en SaleProducts fila {row}");
                            continue;
                        }

                        var key = $"{date:yyyy-MM-dd}|{email.ToLower()}";

                        if (!salesDict.TryGetValue(key, out var sale))
                        {
                            var client = await _users.FindByEmailAsync(email);
                            if (client == null)
                            {
                                errors.Add($"Cliente no encontrado en SaleProducts fila {row}: {email}");
                                continue;
                            }

                            sale = new Sale { Date = date, ClientId = client.Id };
                            _db.Sales.Add(sale);
                            await _db.SaveChangesAsync();

                            salesDict[key] = sale;
                        }

                        var product = await _db.Products.FirstOrDefaultAsync(p => p.Name == productName);
                        if (product == null)
                        {
                            errors.Add($"Producto no encontrado en SaleProducts fila {row}: {productName}");
                            continue;
                        }

                        _db.SaleProducts.Add(new SaleProduct
                        {
                            SaleId = sale.Id,
                            ProductId = product.Id,
                            Quantity = qty,
                            UnitPrice = unitPrice
                        });
                    }

                    await _db.SaveChangesAsync();
                }

                await trx.CommitAsync();
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
