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
    }
}
