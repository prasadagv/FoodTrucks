using FoodTrucks.Tests.Constants;
using Microsoft.AspNetCore.TestHost;
using FoodTrucks.Tests.Shared;

namespace FoodTrucks.Tests.Helper
{
    public static class ServiceTestingHelper
    {
        public static HttpClient BuildServer()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment(TestingConstants.EnvironmentName)
                .UseStartup<TestStartup>();

            var server = new TestServer(builder);

            return server.CreateClient();
        }
    }
}
