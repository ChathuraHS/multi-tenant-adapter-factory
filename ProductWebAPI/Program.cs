using Microsoft.EntityFrameworkCore;
using ProductWebAPI;
using ProductWebAPI.Models;
using ProductWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNameCaseInsensitive = true);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

//Direct microservice to microservice communication
//builder.Services.AddHttpClient("AdapterFactory", client =>
//{
//    client.BaseAddress = new Uri("http://adapter-factory/");
//});

//Ocelot gateway
builder.Services.AddHttpClient("AdapterFactory", c =>
{
    c.BaseAddress = new Uri("http://adapter-factory:80/");
});



//var dbHost = "local";
//var dbName = "dms_tour";
//var dbPassword = "password";
//var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");

var connectionString = $"Server={dbHost};Database={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True;";


builder.Services.AddDbContext<ProductDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddScoped<IProductService, ProductService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseStaticFiles(); //Let ASP.NET serve static files

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    db.Database.Migrate();
}

app.Run();
