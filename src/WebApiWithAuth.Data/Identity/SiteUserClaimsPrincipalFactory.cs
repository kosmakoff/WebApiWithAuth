using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WebApiWithAuth.Data.Entities;

namespace WebApiWithAuth.Data.Identity
{
    public class SiteUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public SiteUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            ((ClaimsIdentity) principal.Identity).AddClaims(new[]
            {
                new Claim(IdentityModel.JwtClaimTypes.GivenName, user.FirstName),
                new Claim(IdentityModel.JwtClaimTypes.FamilyName, user.LastName)
            });

            return principal;
        }
    }
}
