using Warehouse.BLL.Interfaces;
using Warehouse.DAL.DbContext;
using Warehouse.DAL.Interfaces;
using Warehouse.DAL.Repositories;
using Warehouse.BLL.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// ==========================
// Dependency Injection
// ==========================

builder.Services.AddScoped<SqlConnectionFactory>();

// Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();


// Service
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();


// ==========================
// Configure HTTP pipeline
// ==========================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();