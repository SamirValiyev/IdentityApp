using IdentityApp.Dtos;
using IdentityApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto,CancellationToken cancellationToken)
        {
            AppUser user = new();
            user.Email = dto.Email;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.Username;

            IdentityResult identityResult= await userManager.CreateAsync(user,dto.Password);
            if (!identityResult.Succeeded)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto,CancellationToken cancellationToken)
        {
            AppUser? user = await userManager.FindByIdAsync(dto.Id.ToString());
            if(user is null)
                return BadRequest(new {Message="Usern not Found"});
            IdentityResult identityResult= await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors.Select(x => x.Description));
            return NoContent();
        }
    }
}
