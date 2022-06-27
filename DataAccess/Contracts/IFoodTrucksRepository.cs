using FoodTrucks.DataAccess.Entities;
using FoodTrucks.DataAccess.Models;

namespace FoodTrucks.DataAccess.Contracts
{
    public interface IFoodTrucksRepository
    {
        Task<List<FoodFacility>> GetFoodTrucksNoTrackingAsync(FoodTrucksRequest request);
    }
}
