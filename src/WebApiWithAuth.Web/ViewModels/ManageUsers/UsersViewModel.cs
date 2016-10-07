using System.Collections.Generic;
using WebApiWithAuth.Web.ViewModels.DTO;

namespace WebApiWithAuth.Web.ViewModels.ManageUsers
{
    public class UsersViewModel
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
