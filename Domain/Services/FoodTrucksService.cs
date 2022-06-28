using FoodTrucks.DataAccess.Contracts;
using FoodTrucks.DataAccess.Models;
using FoodTrucks.Domain.Contracts;
using FoodTrucks.Domain.Models;

namespace FoodTrucks.Domain.Services
{
    public class FoodTrucksService : IFoodTrucksService
    {
        private readonly IFoodTrucksRepository _repository;

        public FoodTrucksService(IFoodTrucksRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Domain layer which accepts the user input and calls repository layer
        /// </summary>
        /// <param name="FoodTrucksRequestModel">A Model to pass Longitude, Latitude, 
        /// RadiusInMeters and SearchFoodItem</param>
        /// <returns>List of FoodTrucks</returns>
        public async Task<List<FoodTrucksResponseModel>> GetFoodTrucksAsync(FoodTrucksRequestModel request)
        {
            var foodFacilities = await _repository.GetFoodTrucksNoTrackingAsync(new FoodTrucksRequest() 
            { 
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                RadiusInMeters = request.RadiusInMeters,
                SearchFoodItem = request.SearchFoodItem,
            });

            return (foodFacilities != null ? foodFacilities.ConvertAll(x => (FoodTrucksResponseModel)x) : new List<FoodTrucksResponseModel>());
        }
    }
}
