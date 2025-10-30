using AdminManager.Web.Configs;
using AdminManager.Web.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase();

// Use identity user system and role system
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>() // Using our dbContext
    .AddDefaultTokenProviders() // Using tokens for reset or confirmation
    .AddDefaultUI(); // Use razor pages

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
