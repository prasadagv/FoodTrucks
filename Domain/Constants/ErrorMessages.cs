namespace FoodTrucks.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string CoordinatesValidateFailure = "Latitude or Longitude parameters are out of range";

        public const string RadiusValidateFailure = "Radius value can't be more than 100000";
    }
}
