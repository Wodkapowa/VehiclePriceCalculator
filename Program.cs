using Microsoft.OpenApi.Models;
using VehiclePriceCalculator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// <-- Register your service here -->
builder.Services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Vehicle Price Calculator API",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "VehiclePriceCalculator API V1");
        options.RoutePrefix = string.Empty;  // This will make Swagger UI available at the root (https://localhost:7001/)
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();