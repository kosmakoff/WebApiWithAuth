using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace WebApiWithAuth.Models
{
    public class MyRoleStore<TRole, TContext, TKey> : RoleStore<TRole, TContext, TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {
        public MyRoleStore(TContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public override async Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var roleId = ConvertIdFromString(id);
            return await Roles
                .Include(r => r.Claims)
                .FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
        }
    }
}
