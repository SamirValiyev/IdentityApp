using IdentityApp.Context;
using IdentityApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController(AppDbContext appDbContext,UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(Guid userId,/*Guid roleId*/ string roleName,CancellationToken cancellationToken)
        {
            //AppUserRoles appUserRoles = new()
            //{
            //    RoleId = roleId,
            //    UserId = userId,
            //};
            //await appDbContext.UserRoles.AddAsync(appUserRoles);
            //await appDbContext.SaveChangesAsync(cancellationToken);  
            //return NoContent();

            AppUser appUser = await userManager.FindByIdAsync(userId.ToString());
            if (appUser is null) return BadRequest(new { Message = "User not found" });

            IdentityResult identityResult = await userManager.AddToRoleAsync(appUser, roleName);
            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors.Select(x => x.Description));
            return NoContent();
        }

    }
}
