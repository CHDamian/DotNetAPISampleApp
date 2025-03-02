using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Interfaces.IService.IdentityInterfaces;
using DotNetAPISampleApp.Models.DTOs.IdentityDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace DotNetAPISampleApp.Controllers.IdentityControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _identityService.RegisterUserAsync(model.Email, model.Username, 
                model.Password, model.Name, model.Surname, model.PESEL, model.Role);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (User.Identity?.IsAuthenticated == true)
                return BadRequest(Result.Fail("Already logged in."));
            var result = await _identityService.LoginUserAsync(model.UsernameOrEmail, model.Password);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _identityService.LogoutUserAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != model.UserId)
                return Forbid("You can only change your own data.");


            var result = await _identityService.ChangePasswordAsync(model.UserId, 
                model.OldPassword, model.NewPassword);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("change-personal-data")]
        [Authorize]
        public async Task<IActionResult> ChangePersonalData([FromBody] ChangePersonalDataDto model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Administrator");

            if (!isAdmin && userId != model.UserId)
                return Forbid("You can only change your own data unless you are an administrator.");

            var result = await _identityService.ChangePersonalDataAsync(model.UserId, model.Name, model.Surname);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("change-sensitive-data")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangeSensitiveData([FromBody] ChangeSensitiveDataDto model)
        {
            var result = await _identityService.ChangeSensitiveDataAsync(model.UserId, 
                model.Email, model.PESEL);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("set-role")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetRole([FromBody] SetRoleDto model)
        {
            var result = await _identityService.SetUserRoleAsync(model.UserId, model.Role);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _identityService.GetAllUsersAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
