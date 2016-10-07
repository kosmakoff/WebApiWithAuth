using System.Collections.Generic;

namespace WebApiWithAuth.Web.ViewModels.DTO
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}
