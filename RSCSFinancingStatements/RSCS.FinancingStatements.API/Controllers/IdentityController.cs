using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RSCS.FinancingStatements.Core.Models.BusinessObjects.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace RSCS.FinancingStatements.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IConfiguration configuration, ILogger<IdentityController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        
        [HttpPost]
        [Route("token")]
        public IActionResult GenerateAccessToken([FromHeader(Name = "X-API-KEY")] string apiKey)
        {
            _logger.LogInformation($"IdentityController GenerateAccessToken");

            var validApiKey = _configuration["ApiKey"];
            if (apiKey != validApiKey)
            {
                return Unauthorized("Invalid API Key");
            }

            // Generate JWT Token
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, "APIUser"), // Use a generic identifier for API users
                    new Claim(JwtRegisteredClaimNames.Email, string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10), // Adjust token expiry as needed
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new CustomPrincipalClaim
            {
                IsAuthenticated = true,
                UserName = "APIUser",
                AccessToken = jwtToken
            });
        }
    }
}
