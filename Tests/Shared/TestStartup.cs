using FoodTrucks.DataAccess.Contracts;
using FoodTrucks.DataAccess.Repositories;
using FoodTrucks.Domain.Contracts;
using FoodTrucks.Domain.Services;
using FoodTrucks.WebAPI.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Reflection;
using System.Text;

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration.GetSection("JwtToken").GetValue<string>("Issuer"),
                    ValidAudience = _configuration.GetSection("JwtToken").GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtToken").GetValue<string>("Key")))
                };
            });
            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseAuthentication();

        }
    }
}
