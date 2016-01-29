using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace WebApiWithAuth.Models
{
    public class MyUserStore<TUser, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        public MyUserStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public override async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var id = ConvertIdFromString(userId);

            return await base.Users
                .Include(user => user.Roles)
                .Include(user => user.Claims)
                .FirstOrDefaultAsync(u => u.Id.Equals(id), cancellationToken);
        }
    }
}
