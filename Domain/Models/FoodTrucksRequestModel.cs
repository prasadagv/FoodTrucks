using System.ComponentModel.DataAnnotations;

namespace FoodTrucks.Domain.Models
{
    public class FoodTrucksRequestModel
    {
        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public long RadiusInMeters { get; set; }

        public string SearchFoodItem { get; set; }
    }
}
