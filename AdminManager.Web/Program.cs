using AdminManager.Web.Configs;
using AdminManager.Web.Data;
using AdminManager.Web.Data.Seeders;
using AdminManager.Web.Services;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabase();

ExcelPackage.License.SetNonCommercialPersonal("FirmezaApp");
QuestPDF.Settings.License = LicenseType.Community;

// Use identity user system and role system
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>() // Using our dbContext
    .AddDefaultTokenProviders() // Using tokens for reset or confirmation
    .AddDefaultUI(); // Use razor pages

builder.Services.AddScoped<ExcelImportService>();

builder.Services.AddScoped<AllSalesPdfService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Use seed to create default admin + roles
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
await IdentitySeed.SeedAsync(services);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
