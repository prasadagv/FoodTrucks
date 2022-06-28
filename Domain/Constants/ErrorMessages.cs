namespace FoodTrucks.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string CoordinatesValidateFailure = "Latitude or Longitude parameters are out of range";

        public const string RadiusValidateFailure = "Radius value range must be between 1 and 100000";
    }
}
