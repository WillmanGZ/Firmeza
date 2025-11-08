using Firmeza.API.Configs;
using Firmeza.API.Data;
using Firmeza.API.Interfaces;
using Firmeza.API.Repositories;
using Firmeza.API.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.AddScoped<ISaleProductRepository, SaleProductRepository>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
