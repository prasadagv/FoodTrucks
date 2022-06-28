using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FoodTrucks.Domain.Contracts;
using FoodTrucks.Domain.Models;
using FoodTrucks.Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FoodTrucks.Domain.Authorisation;

namespace FoodTrucks.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class FoodTrucksController : Controller
    {
        private readonly ILogger<FoodTrucksController> _logger;
        private readonly IFoodTrucksService _service;

        public FoodTrucksController(IFoodTrucksService service, ILogger<FoodTrucksController> logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpGet("v1/getFoodTrucks")]
        [ProducesResponseType(typeof(List<FoodTrucksResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AuthoriseAdministratorViaJwtBearerTokenAttribute]
        public async Task<IActionResult> GetFoodTrucks([FromQuery] FoodTrucksRequestModel request)
        {
            var stopwatch = Stopwatch.StartNew();

            // Call Domain layer for FoodTrucks
            var resp = await _service.GetFoodTrucksAsync(request);

            //Stop the watch and log the latency as info
            stopwatch.Stop();
            _logger.LogInformation($"{nameof(GetFoodTrucks)} was called successfully. Request Processed in {(int)stopwatch.ElapsedMilliseconds} Milliseconds");

            // Write Pagination/TotalItems as Http Header
            Response.AddPagination(resp.Count);

            // If No items found, return NotFound (http 404)
            if (resp.Count == 0)
            {
                return NotFound();
            }

            // Return Ok (http 200)
            return Ok(resp);
        }
    }
}
