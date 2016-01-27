using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiWithAuth.Models;
using WebApiWithAuth.ViewModels.DTO;
using WebApiWithAuth.ViewModels.ManageUsers;

namespace WebApiWithAuth.Controllers
{
    public class ManageUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ManageUsersController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
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
    }
}
