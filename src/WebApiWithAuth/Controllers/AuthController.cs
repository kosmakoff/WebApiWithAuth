using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiWithAuth.ViewModels.Api;

namespace WebApiWithAuth.Controllers
{
	[Route("api/[controller]")]
	public class AuthController : Controller
    {
        [HttpGet("get_auth_info")]
        public GetAuthInfoViewModel GetAuthInfo()
        {
            return new GetAuthInfoViewModel
            {
                IsAuthenticated = HttpContext.User.Identities.Any(identity => identity.IsAuthenticated),
                SomeTextData = $"The time is {DateTime.Now}"
            };
        }
    }
}
