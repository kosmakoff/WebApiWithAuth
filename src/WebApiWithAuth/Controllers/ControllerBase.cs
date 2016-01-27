using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiWithAuth.Models;

namespace WebApiWithAuth.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public ControllerBase(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        protected async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        }
    }
}
