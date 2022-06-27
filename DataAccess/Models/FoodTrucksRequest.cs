namespace FoodTrucks.DataAccess.Models
{
    public class FoodTrucksRequest
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }
        public long RadiusInMeters { get; set; }

        public string SearchFoodItem { get; set; }
    }
}
