using Identity.Domain.AggregateModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolOfRock.Api.Services
{
    public class TokenGenerator(IConfiguration configuration) : ITokenGenerator
    {
        public string GerarToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Segredo"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracao = DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiracao,
                signingCredentials: creds,
                issuer: configuration["Jwt:Emissor"],
                audience: configuration["Jwt:Audiencia"]
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}