using System.Text.Json;

namespace FoodTrucks.Domain.Models
{
    public class ErrorDetails
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
