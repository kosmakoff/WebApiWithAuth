using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApiWithAuth.Data.Entities
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
