using IdentityApp.Dtos;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager) : ControllerBase
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
            if (!identityResult.Succeeded) return BadRequest();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto,CancellationToken cancellationToken)
        {
            AppUser? user = await userManager.FindByIdAsync(dto.Id.ToString());
            if(user is null) return BadRequest(new { Message = "User not Found" });
            IdentityResult identityResult= await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors.Select(x => x.Description));
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword(string email,CancellationToken cancellationToken)
        {
            AppUser? user=await userManager.FindByEmailAsync(email);
            if (user is null) return BadRequest(new { Message = "User not Found" });
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new {Token=token});
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordWithToken(ChangePasswordWithTokenDto dto,CancellationToken cancellationToken)
        {
        
            AppUser? user=await userManager.FindByEmailAsync(dto.Email);
            if (user is null) return BadRequest(new { Message = "User not Found" });
            IdentityResult identityResult=await userManager.ResetPasswordAsync(user,dto.Token,dto.NewPassword);
            if (!identityResult.Succeeded)
                return BadRequest(identityResult.Errors.Select(x => x.Description));
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto,CancellationToken cancellationToken)
        {
            AppUser? user=await userManager.Users.FirstOrDefaultAsync(x=>x.Email==dto.UsernameOrEmail || x.UserName==dto.UsernameOrEmail,cancellationToken);
            if(user is null) return BadRequest(new { Message = "User not Found" });
            bool result=await userManager.CheckPasswordAsync(user,dto.Password);
            if (result is false) return BadRequest(new { Message = "Password is incorrect" });
            return Ok(new { Token = "Token" });
        }

        [HttpPost]
        public async Task<IActionResult> LoginWithSignInManager(LoginDto dto,CancellationToken cancellationToken)
        {
            AppUser? user=await userManager.Users.FirstOrDefaultAsync(x=>x.Email==dto.UsernameOrEmail || x.UserName==dto.UsernameOrEmail, cancellationToken);
            if (user is null) return BadRequest(new { Message = "User not Found" });
            SignInResult result= await signInManager.PasswordSignInAsync(user, dto.Password,false,true);
            //if (!user.EmailConfirmed)
            //{
            //    user.EmailConfirmed = true;
            //    await userManager.UpdateAsync(user);
            //}
            if (result.IsLockedOut)
            {
                TimeSpan? timeSpan = user.LockoutEnd - DateTime.UtcNow;
                if (timeSpan is not null) return StatusCode(500, $"You have been blocked for {timeSpan.Value.TotalSeconds} seconds because you have logged in 3 times.");  
            }
            if (!result.Succeeded) return StatusCode(500, "Your password is incorrect.");
            if (result.IsNotAllowed) return StatusCode(500, "Your email address is not confirmed.");
            return Ok(new { Token = "Token" });
        }
    }
}
