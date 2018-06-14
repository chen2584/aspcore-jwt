using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace testJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //identity.AddClaim(new Claim(ClaimTypes.Country, "Thailand"));
            foreach(var claim in User.Claims)
            {
                Console.WriteLine("Claim: " + claim.Type.ToString() + " Value: " + claim.Value.ToString());
            }
            var contain = User.Claims.Any(claim => claim.Type == ClaimTypes.Role);
            var name = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Role));
            
            Console.WriteLine(User.Identity.Name); //ClaimTypes.Name
            Console.WriteLine("Developer Role: " + User.IsInRole("Developer"));
            return new string[] { "value1", "value2", User.Claims.Count().ToString(), contain.ToString(), User.FindFirst(ClaimTypes.Role)?.Value };
        }

        // GET api/values/5
        [Authorize(Roles = "User, Developer")]
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
