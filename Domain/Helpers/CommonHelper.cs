using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace FoodTrucks.Domain.Helpers
{
    public static class CommonHelper
    {
        public static void AddPagination(this HttpResponse response, int totalItems)
        {
            var paginationHeader = new
            {
                TotalItems = totalItems,
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
