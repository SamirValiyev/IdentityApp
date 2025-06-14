using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Context;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid,IdentityUserClaim<Guid>,AppUserRoles,IdentityUserLogin<Guid>,IdentityRoleClaim<Guid>,IdentityUserToken<Guid>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AppUserRoles>().HasKey(x => new
        {
           x.RoleId,x.UserId   //composite keys
        });
        builder.Entity<IdentityUserLogin<Guid>>().HasKey(x=>x.UserId);
        builder.Entity<IdentityUserToken<Guid>>().HasKey(x => x.UserId);

    }
}
