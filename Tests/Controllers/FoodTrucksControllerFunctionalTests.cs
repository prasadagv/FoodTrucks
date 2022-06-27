using Xunit;
using FoodTrucks.Tests.Helper;
using FoodTrucks.Tests.Shared;
using System.Net;
using FoodTrucks.WebAPI.Controllers;
using System.Text.Json;
using FoodTrucks.Domain.Models;

namespace FoodTrucks.Tests.Controllers
{
    // TODO: Mock the CosmosClient/Container
    public class FoodTrucksControllerFunctionalTests
    {
        [Fact]
        [Trait(nameof(FoodTrucksController), nameof(FoodTrucksController.GetFoodTrucks))]
        public async void SendNotFoundRequestForFoodTrucks_ReturnNotFound_AndReceiveEmptyFoodTrucks()
        {
            // Given
            var client = ServiceTestingHelper.BuildServer();
            var request = RequestBuilder.BuildGetFoodTrucksRequest(-122.41, 37.81, 1, string.Empty);

            // When
            var response = await client.SendAsync(request);

            // Then
            // Check StatusCode
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        [Trait(nameof(FoodTrucksController), nameof(FoodTrucksController.GetFoodTrucks))]
        public async void SendRequestForFoodTrucks_ReturnOk_AndReceiveFoodTrucks()
        {
            // Given
            var client = ServiceTestingHelper.BuildServer();            
            var request = RequestBuilder.BuildGetFoodTrucksRequest(-122.41, 37.81, 2000, string.Empty);

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