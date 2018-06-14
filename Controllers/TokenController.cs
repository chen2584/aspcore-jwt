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
                claims: new Claim[]
                {
                    //Add more Claims
                    new Claim(JwtRegisteredClaimNames.Sub, "Chen Angolo"),
                    new Claim(JwtRegisteredClaimNames.Email,"worameth.semapat@gmail.com"),
                    new Claim(JwtRegisteredClaimNames.Website, "chen2584.github.io"),
                    new Claim(ClaimTypes.Role, "Developer"),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim(ClaimTypes.Name, "Chen Angelo")

                }
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}