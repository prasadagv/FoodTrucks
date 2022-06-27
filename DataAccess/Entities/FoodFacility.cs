namespace FoodTrucks.DataAccess.Entities
{
    public class FoodFacility
    {
        public string id { get; set; }
        public string objectid { get; set; }
        public string Discriminator { get; set; }
        public string applicant { get; set; }
        public string facilitytype { get; set; }
        public string locationdescription { get; set; }
        public string fooditems { get; set; }
        public string address { get; set; }
        public Location location { get; set; }
    }
}
