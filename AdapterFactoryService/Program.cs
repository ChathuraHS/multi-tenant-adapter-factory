using AdapterFactoryService.Adapters.Factories;
using AdapterFactoryService.Adapters.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Http clients
builder.Services.AddHttpClient("BokunAdapter", c =>
{
    c.BaseAddress = new Uri("http://bokun-adapter");
});
builder.Services.AddHttpClient("AdapterFactory", c =>
{
    c.BaseAddress = new Uri("http://adapter-factory:80/");
});


// Register adapters
builder.Services.AddTransient<BokunProductProviderAdapter>();
builder.Services.AddSingleton<IProductAdapterFactory, ProductAdapterFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
