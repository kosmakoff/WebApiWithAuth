using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebApiWithAuth.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MinLength(2), MaxLength(100)]
        public string FirstName { get; set; }

        [MinLength(2), MaxLength(100)]
        public string LastName { get; set; }
    }
}
