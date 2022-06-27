using FoodTrucks.DataAccess.Contracts;
using FoodTrucks.DataAccess.Repositories;
using FoodTrucks.Domain.Contracts;
using FoodTrucks.Domain.Services;
using FoodTrucks.WebAPI.Controllers;
using FoodTrucks.WebAPI.MiddlewareExtensions;
using NLog.Web;

namespace FoodTrucks.WebAPI
{
    public partial class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddSingleton<ILogger, Logger<FoodTrucksController>>();
builder.Services.AddScoped<IFoodTrucksService, FoodTrucksService>();
builder.Services.AddSingleton<IFoodTrucksRepository>(options =>
{
    var accountEndPoint = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("accountEndPoint");
    var accountKey = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("accountKey");
    var databaseName = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("databaseName");
    var containerName = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("containerName");    

    return new FoodTrucksRepository(accountEndPoint, accountKey, databaseName, containerName);
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

        }
    }
}
