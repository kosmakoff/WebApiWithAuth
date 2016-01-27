using System.Collections.Generic;
using WebApiWithAuth.ViewModels.DTO;

namespace WebApiWithAuth.ViewModels.ManageUsers
{
    public class UsersViewModel
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
