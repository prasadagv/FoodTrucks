using FoodTrucks.DataAccess.Contracts;
using FoodTrucks.DataAccess.Repositories;
using FoodTrucks.Domain.Contracts;
using FoodTrucks.Domain.Services;
using FoodTrucks.WebAPI.Controllers;
using Moq;
using System.Reflection;

namespace FoodTrucks.Tests.Shared
{
    public class TestStartup
    {
        private IConfiguration _configuration;

        public TestStartup()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
        }               

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(Assembly.Load("FoodTrucks.WebAPI")).AddControllersAsServices();

            services.AddSingleton<ILogger, Logger<FoodTrucksController>>();
            services.AddScoped<IFoodTrucksService, FoodTrucksService>();

            // TODO: Need to replace and with Mock the CosmosClient/Container
            services.AddSingleton<IFoodTrucksRepository>(options =>
            {
                var accountEndPoint = _configuration.GetSection("CosmosDBSettings").GetValue<string>("accountEndPoint");
                var accountKey = _configuration.GetSection("CosmosDBSettings").GetValue<string>("accountKey");
                var databaseName = _configuration.GetSection("CosmosDBSettings").GetValue<string>("databaseName");
                var containerName = _configuration.GetSection("CosmosDBSettings").GetValue<string>("containerName");

                return new FoodTrucksRepository(accountEndPoint, accountKey, databaseName, containerName);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
