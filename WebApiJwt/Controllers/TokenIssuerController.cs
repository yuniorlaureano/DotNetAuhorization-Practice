using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiJwt.Models;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace WebApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenIssuerController : ControllerBase
    {
        private IConfiguration _configuration;

        public TokenIssuerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GetToken([FromBody] TokenRequest request)
        {
            if (request.UserName == "Yunior" && request.Password == "123")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.UserName),
                    new Claim(CustomClaims.CompletedBasicTraining, ""),
                    new Claim(CustomClaims.EmploymentCommenced,
                             new DateTime(2017,12,1).ToString(),
                            ClaimValueTypes.DateTime)
                };

                var secret = _configuration["SecurityKey"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256 );

                var token = new JwtSecurityToken(
                    issuer : "yuni@gmail.com",
                    audience: "yuni@gmail.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: creds
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });

            }

            return BadRequest("Could not verify userName and passWord");
        }
    }
}