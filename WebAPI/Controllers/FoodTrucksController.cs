using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FoodTrucks.Domain.Contracts;
using FoodTrucks.Domain.Models;
using FoodTrucks.Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FoodTrucks.Domain.Authorisation;
using FoodTrucks.Domain.Constants;

namespace FoodTrucks.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class FoodTrucksController : Controller
    {
        private readonly ILogger<FoodTrucksController> _logger;
        private readonly IFoodTrucksService _service;

        public FoodTrucksController(IFoodTrucksService service, ILogger<FoodTrucksController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get FoodTrucks data based on Longitude, Latitude, RadiusInMeters
        /// and optional SearchFoodItem to filter the data.
        /// </summary>
        /// <param name="FoodTrucksRequestModel">A Model to pass Longitude, Latitude, 
        /// RadiusInMeters and SearchFoodItem</param>
        /// <returns>List of Food Trucks</returns>
        [HttpGet("v1/getFoodTrucks")]
        [ProducesResponseType(typeof(List<FoodTrucksResponseModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AuthoriseAdministratorViaJwtBearerTokenAttribute]
        public async Task<IActionResult> GetFoodTrucks([FromQuery] FoodTrucksRequestModel request)
        {
            var stopwatch = Stopwatch.StartNew();

            // Validate Latitude & Longitude and send BadRequest (400) if validation fails
            if (!CommonHelper.ValidateCoordinates(request))
            {
                return BadRequest(ErrorMessages.CoordinatesValidateFailure);
            }

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
