using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiWithAuth.Models;
using WebApiWithAuth.ViewModels.DTO;
using WebApiWithAuth.ViewModels.ManageUsers;

namespace WebApiWithAuth.Controllers
{
    [Authorize]
    public class ManageUsersController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageUsersController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
            : base(userManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IActionResult> ShowUsers()
        {
            var usersModel = new UsersViewModel();

            foreach (var user in _userManager.Users.ToList())
            {
                usersModel.Users.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = (await _userManager.GetRolesAsync(user)).ToList()
                });
            }

            return View(usersModel);
        }

        public async Task<IActionResult> AssignRoles()
        {
            var user = await GetCurrentUserAsync();

            if (!(await _roleManager.RoleExistsAsync("Administrators")))
            {
                var role = new ApplicationRole() { Name = "Administrators" };
                role.Claims.Add(new IdentityRoleClaim<string> { ClaimType = ClaimTypes.PrimarySid, ClaimValue = "001" });

                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, "Administrators");

            return RedirectToAction(nameof(ShowUsers));
        }
    }
}
