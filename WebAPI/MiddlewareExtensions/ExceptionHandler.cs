using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using FoodTrucks.Domain.Models;

namespace FoodTrucks.WebAPI.MiddlewareExtensions
{
    public static class ExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder appBuilder, ILogger logger)
        {
            appBuilder.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {
                        logger.LogError($"Error Occured: {exceptionHandlerFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            ErrorCode = context.Response.StatusCode,
                            ErrorMessage = "Internal Server Error"
                        }.ToString());
                    }
                });
            });
        }
    }
}
