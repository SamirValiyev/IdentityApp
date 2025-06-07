using IdentityApp.Context;
using IdentityApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController(AppDbContext appDbContext) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(Guid userId,Guid roleId,string roleName,CancellationToken cancellationToken)
        {
            AppUserRoles appUserRoles = new()
            {
                RoleId = roleId,
                UserId = userId,
            };
            await appDbContext.UserRoles.AddAsync(appUserRoles);
            await appDbContext.SaveChangesAsync(cancellationToken);  
            return NoContent();
        }

    }
}
