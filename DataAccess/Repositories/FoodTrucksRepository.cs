using FoodTrucks.DataAccess.Contracts;
using FoodTrucks.DataAccess.Entities;
using FoodTrucks.DataAccess.Models;
using Microsoft.Azure.Cosmos;
using System.Text.Json;

namespace FoodTrucks.DataAccess.Repositories
{
    public class FoodTrucksRepository : IFoodTrucksRepository
    {
        private readonly Container _container;

        public FoodTrucksRepository(string accountEndPoint, string accountKey, string databaseName, string containerName)
        {
            var cosmosClient = new CosmosClient(accountEndPoint, accountKey);
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<List<FoodFacility>> GetFoodTrucksNoTrackingAsync(FoodTrucksRequest request)
        {
            var query = "select f.id, f.objectid, f.applicant, f.Discriminator, f.facilitytype, f.locationdescription, f.fooditems, f.address, f.location FROM c f WHERE ST_DISTANCE(f.location, {0}) < {1} AND f.facilitytype = 'Truck' {2}";
            Location loc1 = new()
            {
                coordinates = new List<double>()
            };
            loc1.coordinates.Add(request.Longitude);
            loc1.coordinates.Add(request.Latitude);
            loc1.type = "Point";

            string subQuery = " and contains(upper(f.fooditems), upper('{0}'))";
            if (!string.IsNullOrWhiteSpace(request.SearchFoodItem))
            {
                subQuery = string.Format(subQuery, request.SearchFoodItem);
            }
            else
            {
                subQuery = string.Empty;
            }

            query = string.Format(query, JsonSerializer.Serialize(loc1), request.RadiusInMeters, subQuery);

            var queryResultIterator = _container.GetItemQueryIterator<FoodFacility>(new QueryDefinition(query));
            var foodTrucks = new List<FoodFacility>();

            while (queryResultIterator != null && queryResultIterator.HasMoreResults)
            {
                var res = await queryResultIterator.ReadNextAsync();
                foodTrucks.AddRange(res);
            }

            return foodTrucks;
        }
    }
}
