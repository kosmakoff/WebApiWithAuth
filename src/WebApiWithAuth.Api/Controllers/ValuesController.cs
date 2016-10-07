using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiWithAuth.Api.Controllers
{
    [Route("[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<string>> List()
        {
            var results = new List<string> {"first", "second", "last"};
            return results;
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            if (id != 42)
                return NotFound();

            return Ok(100500);
        }

        [HttpGet]
        [Route("claims")]
        [Authorize]
        public async Task<IActionResult> GetClaims()
        {
            return Json(new {Claims = User.Claims.Select(claim => new {Type = claim.Type, Value = claim.Value})});
        }
    }
}
