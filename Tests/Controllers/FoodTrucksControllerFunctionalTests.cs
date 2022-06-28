using Xunit;
using FoodTrucks.Tests.Helper;
using FoodTrucks.Tests.Shared;
using System.Net;
using FoodTrucks.WebAPI.Controllers;
using System.Text.Json;
using FoodTrucks.Domain.Models;
using FoodTrucks.Domain.Constants;

namespace FoodTrucks.Tests.Controllers
{
    // TODO: Mock the CosmosClient/Container
    public class FoodTrucksControllerFunctionalTests
    {
        private readonly IConfiguration _configuration;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;

        public FoodTrucksControllerFunctionalTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            _issuer = _configuration.GetSection("JwtToken").GetValue<string>("Issuer");
            _audience = _configuration.GetSection("JwtToken").GetValue<string>("Audience");
            _key = _configuration.GetSection("JwtToken").GetValue<string>("Key");
        }

        [Fact]
        [Trait(nameof(FoodTrucksController), nameof(FoodTrucksController.GetFoodTrucks))]
        public async void SendRequestWithNoToken_ReturnUnauthorised_AndReceiveUnauthorised()
        {
            // Given
            var client = ServiceTestingHelper.BuildServer();

            var token = string.Empty;

            var request = RequestBuilder.BuildGetFoodTrucksRequest(token, -122.41, 37.81, 2000, string.Empty);

            // When
            var response = await client.SendAsync(request);

            // Then
            // Check StatusCode
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [Trait(nameof(FoodTrucksController), nameof(FoodTrucksController.GetFoodTrucks))]
        public async void SendValidRequestByUser_ReturnForbidden_AndReceiveForbidden()
        {
            // Given
            var client = ServiceTestingHelper.BuildServer();

            var token = GenerateToken.GenerateJwtToken(ApiRoles.User, _issuer, _audience, _key);

            var request = RequestBuilder.BuildGetFoodTrucksRequest(token, -122.41, 37.81, 2000, string.Empty);

            // When
            var response = await client.SendAsync(request);

            // Then
            // Check StatusCode
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [Trait(nameof(FoodTrucksController), nameof(FoodTrucksController.GetFoodTrucks))]
        public async void SendNotFoundRequestByAdministrator_ReturnNotFound_AndReceiveEmptyFoodTrucks()
        {
            // Given
            var client = ServiceTestingHelper.BuildServer();            

            var token = GenerateToken.GenerateJwtToken(ApiRoles.Administrator, _issuer, _audience, _key);

            var request = RequestBuilder.BuildGetFoodTrucksRequest(token , - 122.41, 37.81, 1, string.Empty);

            // When
            var response = await client.SendAsync(request);

            // Then
            // Check StatusCode
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait(nameof(FoodTrucksController), nameof(FoodTrucksController.GetFoodTrucks))]
        public async void SendValidRequestByAdministrator_ReturnOk_AndReceiveFoodTrucks()
        {
            // Given
            var client = ServiceTestingHelper.BuildServer();

            var token = GenerateToken.GenerateJwtToken(ApiRoles.Administrator, _issuer, _audience, _key);

            var request = RequestBuilder.BuildGetFoodTrucksRequest(token, -122.41, 37.81, 2000, string.Empty);

            // When
            var response = await client.SendAsync(request);

            // Then
            // Check StatusCode
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseModel = JsonSerializer.Deserialize<List<FoodTrucksResponseModel>>(await response.Content.ReadAsStringAsync());
            
            Assert.NotEmpty(responseModel);
        }
    }
}