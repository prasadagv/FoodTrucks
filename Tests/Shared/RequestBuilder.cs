using Microsoft.Net.Http.Headers;

namespace FoodTrucks.Tests.Shared
{
    public static class RequestBuilder
    {
        public static HttpRequestMessage BuildGetFoodTrucksRequest(string token, double longitude, double latitude, long radiusInMeters, string searchFoodItem)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/v1/getFoodTrucks?longitude={longitude}&latitude={latitude}&radiusInMeters={radiusInMeters}&searchFoodItem={searchFoodItem}");
            request.Headers.Add(HeaderNames.Authorization, $"Bearer {token}");

            return request;
        }
    }
}
