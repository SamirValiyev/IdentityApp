using IdentityApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController(RoleManager<AppRole> roleManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(string name,CancellationToken cancellationToken)
        {
            AppRole role = new()
            {
                Name = name,
            };
            IdentityResult identityResult=await roleManager.CreateAsync(role);  
            if(!identityResult.Succeeded) return BadRequest(identityResult.Errors.Select(x=>x.Description));
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var roles=await roleManager.Roles.ToListAsync(cancellationToken);
            return Ok(roles);
        }


    }
}
