using Microsoft.IdentityModel.Tokens;
using Register_Supply_Management.Contracts.Handler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Register_Supply_Management.Utilities.Handlers
{
    public class TokenHandlers : ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandlers(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTService:Key"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(issuer: _configuration["JWTService:Issuer"],
                                                    audience: _configuration["JWTService:Audience"],
                                                    claims: claims,
                                                    expires: DateTime.Now.AddMinutes(60),
                                                    signingCredentials: signinCredentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }
    }
}
