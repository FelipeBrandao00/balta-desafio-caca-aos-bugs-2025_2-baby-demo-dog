using BugStore.Data;
using BugStore.Endpoints;
using BugStore.Handlers.Customers;
using BugStore.Handlers.Orders;
using BugStore.Handlers.Products;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CustomerHandler>();
builder.Services.AddScoped<ProductHandler>();
builder.Services.AddScoped<OrderHandler>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BugStore API",
        Version = "v1",
        Description = "API do desafio Caça aos Bugs 2025 - Baby Demo Dog"
    });

    // Avoid schema ID collisions between classes with the same name in different namespaces
    opts.CustomSchemaIds(t => t.FullName!.Replace("+", "."));
});

var app = builder.Build();


// Habilita Swagger no ambiente de desenvolvimento (Debug)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "BugStore API v1");
        opts.DocumentTitle = "BugStore - Swagger";
        // opcional: opts.RoutePrefix = string.Empty; // expõe em "/"
    });
}

app.MapGet("/", () => "Desafio Finalizado!");

CustomerEndpoints.MapCustomerEndpoints(app);

ProductEndpoints.MapProductEndpoints(app);

OrderEndpoints.MapOrderEndpoints(app);

app.Run();
