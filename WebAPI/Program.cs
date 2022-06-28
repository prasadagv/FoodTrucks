using FoodTrucks.DataAccess.Contracts;
using FoodTrucks.DataAccess.Repositories;
using FoodTrucks.Domain.Contracts;
using FoodTrucks.Domain.Services;
using FoodTrucks.WebAPI.Controllers;
using FoodTrucks.WebAPI.MiddlewareExtensions;
using NLog.Web;
using Microsoft.OpenApi.Models;
using FoodTrucks.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = Defaults.ServiceName,
        Version = "v1",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "JWT Autherization header using the Bearer Scheme.  Example: \"Authorization: Bearer {token}\"",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    options.DescribeAllParametersInCamelCase();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtToken").GetValue<string>("Issuer"),
        ValidAudience = builder.Configuration.GetSection("JwtToken").GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtToken").GetValue<string>("Key")))
    };
});
builder.Services.AddAuthorization();


// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddSingleton<ILogger, Logger<FoodTrucksController>>();
builder.Services.AddScoped<IFoodTrucksService, FoodTrucksService>();
builder.Services.AddSingleton<IFoodTrucksRepository>(options =>
{
    var accountEndPoint = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("accountEndPoint");
    var accountKey = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("accountKey");
    var databaseName = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("databaseName");
    var containerName = builder.Configuration.GetSection("CosmosDBSettings").GetValue<string>("containerName");    

    return new FoodTrucksRepository(accountEndPoint, accountKey, databaseName, containerName);
});

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger>();
app.ConfigureExceptionHandler(logger);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
