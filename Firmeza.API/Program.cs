using Firmeza.API.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
