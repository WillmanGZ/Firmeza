using DotNetEnv;
using Firmeza.API.Configs;
using Firmeza.API.Data;
using Firmeza.API.Interfaces;
using Firmeza.API.Repositories;
using Firmeza.API.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Services.AddDatabase();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddScoped<ISaleProductService, SaleProductService>();

builder.Services.AddScoped<ISaleProductRepository, SaleProductRepository>();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("OpenCorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()   // permite cualquier dominio
            .AllowAnyHeader()   // permite cualquier header
            .AllowAnyMethod();  // permite cualquier método HTTP
    });
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    x.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});


var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("OpenCorsPolicy");

app.Run();
