# FoodTrucks

FoodTrucks APIs find the near food trucks based on provided coordinates.

There are 3 APIs for this solution.

1. SyncFoodtrucksData: This API is used to sync the data from "San Francisco's food truck open dataset" to the Cosmos DB.

    Hosted at: https://venkat-syncfoodtrucksdata.azurewebsites.net/swagger/index.html
    
1. Authentication: API to generate JWT token based on provided credentials.

    Hosted at: https://venkat-authentication.azurewebsites.net/swagger/index.html
    
1. Foodtrucks: **Actual API for this problem** to get the list of food trucks based on provided Longitude, Latitude, RadiusInMeters and optional SearchFoodItem.

    Hosted at: https://venkat-foodtrucks.azurewebsites.net/swagger/index.html


# Problem

Our San Francisco team loves to eat. They are also a team that loves variety, so they also like to discover new places to eat.

In fact, we have a particular affection for food trucks. One of the great things about Food Trucks in San Francisco is that the city releases a list of them as open data.

Your assignment is to make it possible for us to find a food truck no matter where our work takes us in the city.

This is a freeform assignment. You can write a web API that returns a set of food trucks (our team is fluent in JSON). You can write a web frontend that visualizes the nearby food trucks. We also spend a lot of time in the shell, so a CLI that gives us a couple of local options would be great. And don't be constrained by these ideas if you have a better one!

The only requirement for the assignment is that it give us at least 5 food trucks to choose from a particular latitude and longitude.

Feel free to tackle this problem in a way that demonstrates your expertise of an area -- or takes you out of your comfort zone. For example, if you build Web APIs by day and want to build a frontend to the problem or a completely different language instead, by all means go for it - learning is a core competency in our group. Let us know this context in your solution's documentation.

San Francisco's food truck open dataset is [located here](https://data.sfgov.org/Economy-and-Community/Mobile-Food-Facility-Permit/rqzj-sfat/data) and there is an endpoint with a [CSV dump of the latest data here](https://data.sfgov.org/api/views/rqzj-sfat/rows.csv). We've included a [copy of this data](./Mobile_Food_Facility_Permit.csv) in this repo as well.


# Solution

## Key Requirements
1. Get at least 5 food trucks to choose from a particular latitude and longitude.
1. Write a web API that returns a set of food trucks (JSON Payload) or develop a web frontend that visualizes the nearby food trucks


## Scope
1. Can get the list of food trucks based on provided Longitude, Latitude, RadiusInMeters and optional SearchFoodItem.  Optional SearchFoodItem parameter is used to filter the data using FoodItems field.
1. Actual Foodtrucks API is secured with JWT token.
1. Integration Tests using xunit are covered in the Foodtrucks api solution.
1. Swagger supported is added, append /swagger to the end of API URLs for the swagger UI.


## Out of Scope
1. Input validations on Longitude, Latitude, RadiusInMeters and optional SearchFoodItem
1. Configuration keys in APIs are not stored/accessed securely using solutions like Azure Key-Vault.
1. Mocking of Cosmos Container and Cosmos Client is not done in Foodtrucks integration test scripts.
1. Geo-redundant application deployment process is not followed.
1. Deployment scripts are not provided.
1. CI/CD is not implemented.
1. Infrastructure scripts are not developed.
1. Non-Functional (Performance, Load, UI...) testing is not done.
1. Design templates/drawings are not documented.

## Rationale
1. **Cosmos DB**: Azure Cosmos DB is a fully managed NoSQL database service for building scalable, high performance applications.
    * Schema design is much easier and flexible
    * Database reads and writes are really fast
    * Auto indexing on all the attributes in a document.
    * Scalable and globally distributed.
    * Better geospatial data support.


## Implementation

1. **Hosted**: All the three APIs (SyncFoodtrucksData, Authentication, Foodtrucks) are hosted as public Web APIs on Microsoft Azure.
1. **Technology stacks**: .NET 6, C# 10, Cosmos DB (with SQL API), XUnit, JWT, JSON, NLog, Swagger.
1. **Authentication/Authorization**: Implemented using JWT tokens.

&nbsp;

### SyncFoodtrucksData API
This API is used to sync the data from "San Francisco's food truck open dataset" to the Cosmos DB.  SyncFoodtrucksData calls data.sfgov.org (https://data.sfgov.org/resource/rqzj-sfat.json) using http API call and stores the response to CosmosDB.

Hosted at: https://venkat-syncfoodtrucksdata.azurewebsites.net/swagger/index.html

Send POST request without any body to: https://venkat-syncfoodtrucksdata.azurewebsites.net/api/v1/syncFoodTrucks

<ins>Please note this API doesn't follow best practices as this is a setup service to run actual Foodtrucks API.</ins>

&nbsp;

### Authentication API

API to generate JWT token based on provided credentials (username and password).  For simplicity purpose username and passwords are defined as a repository model without using any dependencies like DB, AD.

    Hosted at: https://venkat-authentication.azurewebsites.net/swagger/index.html

Send POST request with below body to: https://venkat-authentication.azurewebsites.net/api/v1/login

The API takes UserName and Password as input in body:

1. username
    * Datatype : string
    * Range : admin1, admin2, user1
    
2. password
    * Datatype : string
    * Range : password1, password2, password1

Example:

{
  "username": "admin1",
  "password": "password1"
}

<ins>Defined Username and password in the API</ins>

Admin Users / Passwords: admin1 / password1, admin2 / password2

Normal Users / Passwords: user1 / password1

<ins>Please note this API doesn't follow best practices as this is just to generate JWT token to run actual Foodtrucks API.</ins>

&nbsp;

### Foodtrucks API
**Actual API for this problem** to get the list of food trucks based on provided Longitude, Latitude, RadiusInMeters and optional SearchFoodItem.

    Hosted at: https://venkat-foodtrucks.azurewebsites.net/swagger/index.html

Add JWT bearer token to the header.  Please note only Administrator (admin1 or admin2) users can't access this API.

Send GET request without optional SearchFoodItem: https://venkat-foodtrucks.azurewebsites.net/api/v1/getFoodTrucks?longitude=-122.41&latitude=37.81&radiusInMeters=2000

Send GET request with optional SearchFoodItem: https://venkat-foodtrucks.azurewebsites.net/api/v1/getFoodTrucks?longitude=-122.41&latitude=37.81&radiusInMeters=2000&searchFoodItem=chicken

This API fetches the result from Cosmos DB based on search criteria.

The API takes the following parameters as input:

1. Longitude - search location longitude 
    * Datatype : double
    * Range : 180 to -180
    
2. Latitude - Search location latitude
    * Datatype : double
    * Range : 180 to -180
    
3. RadiusInMeters - Search in the radious of location coordinates
    * Datatype : Long
    * Range : 1 to 100000
    
4. SearchFoodItem - A free form text to mention name of Food Item to search
    * Datatype : String
    * Range : 0 to 100 characters (soft limit)
    * Example: Chicken

&nbsp;


## Steps to Host and Configure

Below are steps to setup Cosmos DB and host the APIs instead of using existing above links. 

### Setup Cosmos DB using Azure CLI

1. Create an Azure subscription if it doesn't exist.
1. Create a resource group if it doesn't exist.
1. Create an Azure Cosmos DB Account with Core (SQL) API (Update resorce group 'RG1' and  subscription 'AzureTestSubscription' with actual names. If required change regionName value from 'West US').

    * az cosmosdb create --name venkat-cosmosdb-acct --resource-group RG1 --locations regionName="West US" failoverPriority=0 isZoneRedundant=False --subscription AzureTestSubscription

1. Create an Azure Cosmos DB database (Update resorce group 'RG1' with actual name).

    * az cosmosdb sql database create --account-name venkat-cosmosdb-acct --name FoodTrucksDB --resource-group RG1

1. Create an Azure Cosmos DB SQL container  (Update resorce group 'RG1' with actual name).

    * az cosmosdb sql container create -g RG1 -a venkat-cosmosdb-acct -d FoodTrucksDB -n FoodFacility --partition-key-path "/status"

<ins>Validate</ins>:

1. In Azure Portal open Azure Cosmos DB Service
1. Click on 'venkat-cosmosdb-acct' account (or the acccount if you changed the name in above scripts)
1. Click on Data Explorer
1. In SQL API / Data, make sure FoodTrucksDB Database exists.
1. Expand FoodTrucksDB database and make sure FoodFacility container exists.


### Setup SyncFoodTrucksData API and Sync the Data

1. In Azure App Services, deploy SyncFoodTrucksData API
1. Define below configurations in SyncFoodTrucksData app service
  
    * CosmosDBSettings__accountEndPoint: <URI value from keys of the newly created Cosmos DB account>
    * CosmosDBSettings__accountKey: <Primary Key value from keys of the newly created Cosmos DB account>
    * CosmosDBSettings__containerName: FoodFacility
    * CosmosDBSettings__databaseName: FoodTrucksDB
    
1. Browse to the swagger UI of new syncfoodtrucksdata api in browser (append /swagger at the end of URL) and execute "/api/v1/syncFoodTrucks" or send POST request without any body to https://<NewURI>.azurewebsites.net/api/v1/syncFoodTrucks if you are using tools like postman and make sure http response code 200 is received.

<ins>Validate</ins>:

1. In Azure Portal open Azure Cosmos DB Service
1. Click on 'venkat-cosmosdb-acct' account (or the acccount if you changed the name in above scripts)
1. Click on Data Explorer
1. In SQL API / Data, expand FoodTrucksDB Database.
1. Expand FoodFacility container.
1. Make sure documents exists in Items


### Setup Authentication API and generate JWR Token

1. In Azure App Services, deploy Authentication API
1. Define below configurations in Authentication app service
  
    * JwtToken__Audience: FoodTrucksWebAPI
    * JwtToken__Issuer:  FoodTrucksWebAPI
    * JwtToken__Key: 70A28AFA-3BCD-4940-84DE-FB5821EE00D4
    
1. Browse to the swagger UI of new Authentication api in browser (append /swagger at the end of URL) and execute "/api/v1/login" with below body or send POST request with below body to https://<NewURL>.azurewebsites.net/api/v1/login if you are using tools like postman and make sure token is received as reponse.

{
  "username": "admin1",
  "password": "password1"
}

<ins>Validate</ins>:

1. Http response 200 received
1. JWT token received as response.


### Setup Foodtrucks API and get the list of Food Trucks

1. In Azure App Services, deploy Foodtrucks API
1. Define below configurations in Foodtrucks app service
  
    * CosmosDBSettings__accountEndPoint: <URI value from keys of the newly created Cosmos DB account>
    * CosmosDBSettings__accountKey: <Primary Key value from keys of the newly created Cosmos DB account>
    * CosmosDBSettings__containerName: FoodFacility
    * CosmosDBSettings__databaseName: FoodTrucksDB
    * JwtToken__Audience: FoodTrucksWebAPI
    * JwtToken__Issuer:  FoodTrucksWebAPI
    * JwtToken__Key: 70A28AFA-3BCD-4940-84DE-FB5821EE00D4
    
1. Browse to the swagger UI of new Foodtrucks api in browser (append /swagger at the end of URL) and execute "/api/v1/getFoodTrucks" with below values or send POST request with below URLs if you are using tools like postman and make sure list of food trucks are received as reponse.

longitude: -122.41
latitude: 37.81
radiusInMeters: 2000
searchFoodItem (optional): Chicken

Send GET request without optional SearchFoodItem: https://venkat-foodtrucks.azurewebsites.net/api/v1/getFoodTrucks?longitude=-122.41&latitude=37.81&radiusInMeters=2000
Send GET request with optional SearchFoodItem: https://venkat-foodtrucks.azurewebsites.net/api/v1/getFoodTrucks?longitude=-122.41&latitude=37.81&radiusInMeters=2000&searchFoodItem=chicken



<ins>Validate</ins>:

1. Http response 200 received
1. List of Food Trucks received as response.


### Important links
* :link: [Differences between NoSQL and relational databases](https://docs.microsoft.com/en-us/azure/cosmos-db/relational-nosql)
* :link: [EF Core Azure Cosmos DB Provider](https://docs.microsoft.com/en-us/ef/core/providers/cosmos/?tabs=dotnet-core-cli)
* :link: [Cosmos DB Tutorial](https://docs.microsoft.com/en-us/azure/cosmos-db/sql/sql-api-get-started)
