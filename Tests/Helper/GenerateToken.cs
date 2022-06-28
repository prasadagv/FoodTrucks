using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodTrucks.Tests.Helper
{
    public static class GenerateToken
    {
        public static string GenerateJwtToken(string apiRoles, string issuer, string audience, string key)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Authentication, "true"),
                new Claim(ClaimTypes.NameIdentifier, "testUser"),
                new Claim(ClaimTypes.Role, apiRoles)
            };

            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(5),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                                                                SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
