using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace testJwt
{
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private IConfiguration Configuration;

        public TokenController(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        [HttpGet]
        public IActionResult CreateToken(string username="admin", string password="admin")
        {
            IActionResult response = Unauthorized();
            if(username.Equals("admin") && password.Equals("admin"))
            {
                var jwtToken = JwtTokenBuilder();
                response = Ok(new { access_token = jwtToken });
            }

            return response;
        }

        private string JwtTokenBuilder()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtAuthentication:SecurityKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

            var jwtToken = new JwtSecurityToken(
                issuer: Configuration["JwtAuthentication:Issuer"],
                audience: Configuration["JwtAuthentication:Audience"],
                signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(30),
                claims: new[]
                {
                    //Add more Claims
                    new Claim(JwtRegisteredClaimNames.Sub, "Sub", "admin"),
                    new Claim(JwtRegisteredClaimNames.Email, "Email","worameth.semapat@gmail.com"),
                    new Claim(JwtRegisteredClaimNames.Website, "Website", "chen2584.github.io"),

                }
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}