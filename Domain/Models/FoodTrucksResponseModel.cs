using FoodTrucks.DataAccess.Entities;

namespace FoodTrucks.Domain.Models
{
    public class FoodTrucksResponseModel
    {
        public string id { get; set; }
        public string objectid { get; set; }
        public string applicant { get; set; }
        public string facilitytype { get; set; }
        public string locationdescription { get; set; }
        public string fooditems { get; set; }
        public string address { get; set; }
        public Location location { get; set; }

        public static explicit operator FoodTrucksResponseModel(FoodFacility foodFacility)
        {
            if (foodFacility == null)
            {
                return null;
            }

            return new()
            {
                id = foodFacility.id,
                objectid = foodFacility.objectid,
                applicant = foodFacility.applicant,
                facilitytype = foodFacility.facilitytype,
                locationdescription = foodFacility.locationdescription,
                fooditems = foodFacility.fooditems,
                address = foodFacility.address,
                location = foodFacility.location,
            };
        }
    }
}
