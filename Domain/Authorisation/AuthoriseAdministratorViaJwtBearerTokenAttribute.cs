using FoodTrucks.Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace FoodTrucks.Domain.Authorisation
{
    public class AuthoriseAdministratorViaJwtBearerTokenAttribute : AuthorizeAttribute
    {
        public AuthoriseAdministratorViaJwtBearerTokenAttribute()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
            Roles = ApiRoles.Administrator;
        }
    }
}
