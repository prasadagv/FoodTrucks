using FoodTrucks.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace FoodTrucks.Domain.Helpers
{
    public static class CommonHelper
    {
        /// <summary>
        /// Adds custom Pagination header to the response
        /// along with setting "Access-Control-Expose-Headers" to make Pagination
        /// as CORS-safelisted response header
        /// </summary>
        /// <param name="HttpResponse">this HttpResponse</param>
        /// <param name="totalItems">Total FoodTrucks items in the response</param>
        public static void AddPagination(this HttpResponse response, int totalItems)
        {
            var paginationHeader = new
            {
                TotalItems = totalItems,
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        /// <summary>
        /// Validate the Longitude and Latitude
        /// </summary>
        /// <param name="FoodTrucksRequestModel">Input model</param>
        /// <exception cref="bool">return true if validation is success else false</exception>
        public static bool ValidateCoordinates(FoodTrucksRequestModel request)
        {
            bool isSuccess = true;

            if (request.Latitude < -90 || request.Latitude > 90 || request.Longitude < -180 || request.Longitude > 180)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Validate the Radius
        /// </summary>
        /// <param name="range">long value</param>
        /// <exception cref="bool">return true if validation is success else false</exception>
        public static bool ValidateRadius(long radiusInMeters)
        {
            bool isSuccess = true;

            if (radiusInMeters > 100000) // in meters
            {
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}
