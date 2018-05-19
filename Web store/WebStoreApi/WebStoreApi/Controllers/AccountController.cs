using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Api.DataTransferObjects;
using WebStore.Models.Entities;

namespace WebStore.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(
            SignInManager<User> signInManager, 
            UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // POST: /Account/Register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginInfoDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, new[] { "User" });
                    return await AuthorizeResultAsync(model, StatusCodes.Status200OK);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // POST: /Account/Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInfoDTO model)
        {
            if (ModelState.IsValid)
            {
                return await AuthorizeResultAsync(model, StatusCodes.Status200OK);
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private async Task<IActionResult> AuthorizeResultAsync(LoginInfoDTO model, int code)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    return StatusCode(code, await MapToProfile(user));
                }
            }

            return Unauthorized();
        }

        private async Task<UserProfileDTO> MapToProfile(User user)
        {
            return new UserProfileDTO()
            {
                Email = user.Email,
                UserName = user.UserName,
                IsAdministrator = await _userManager.IsInRoleAsync(user, "Administrator")
            };
        }
    }
}
