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

        /// <summary>
        /// Takes Longitude, Latitude, RadiusInMeters and SearchFoodItem as input in FoodTrucksRequest.
        /// Calls Cosmos DB using  SDK (CosmosClient) and returns the result
        /// </summary>
        /// <param name="FoodTrucksRequest">A Model to pass Longitude, Latitude, 
        /// RadiusInMeters and SearchFoodItem</param>
        /// <returns>List of Food Trucks</returns>
        public async Task<List<FoodFacility>> GetFoodTrucksNoTrackingAsync(FoodTrucksRequest request)
        {
            // Form basic query
            var query = "select f.id, f.objectid, f.applicant, f.Discriminator, f.facilitytype, f.locationdescription, f.fooditems, f.address, f.location FROM c f WHERE ST_DISTANCE(f.location, {0}) < {1} AND f.facilitytype = 'Truck' {2}";
            Location loc = new()
            {
                coordinates = new List<double>()
            };
            loc.coordinates.Add(request.Longitude);
            loc.coordinates.Add(request.Latitude);
            loc.type = "Point";

            // Form predicate with fooditems field if users SearchFoodItem else assign empty
            string subQuery = " and contains(upper(f.fooditems), upper('{0}'))";
            if (!string.IsNullOrWhiteSpace(request.SearchFoodItem))
            {
                subQuery = string.Format(subQuery, request.SearchFoodItem);
            }
            else
            {
                subQuery = string.Empty;
            }

            // Serialize location and format the query
            query = string.Format(query, JsonSerializer.Serialize(loc), request.RadiusInMeters, subQuery);

            // Call Cosmos DB and return results
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
