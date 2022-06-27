using FoodTrucks.Domain.Models;

namespace FoodTrucks.Domain.Contracts
{
    public interface IFoodTrucksService
    {
        Task<List<FoodTrucksResponseModel>> GetFoodTrucksAsync(FoodTrucksRequestModel request);
    }
}
