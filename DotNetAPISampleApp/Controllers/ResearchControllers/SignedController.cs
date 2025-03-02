using DotNetAPISampleApp.Interfaces.IService.ResearchInterfaces;
using DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO;
using DotNetAPISampleApp.Models.ResearchModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetAPISampleApp.Controllers.ResearchControllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SignedController : ControllerBase
    {
        private readonly ISigningService _signingService;

        public SignedController(ISigningService signingService)
        {
            _signingService = signingService;
        }

        [HttpPost("researcher")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> SignAsResearcher([FromBody] SignRequestDto request)
        {
            var result = await _signingService.SignToResearchAsync(request.UserId, 
                request.ResearchId, ResearchRole.Researcher);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("patient")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> SignAsPatient([FromBody] SignRequestDto request)
        {
            var result = await _signingService.SignToResearchAsync(request.UserId, 
                request.ResearchId, ResearchRole.Patient);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{signedId}")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> RemoveSigned(int signedId)
        {
            var result = await _signingService.RemoveFromResearchAsync(signedId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{signedId}")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> GetSignedById(int signedId)
        {
            var result = await _signingService.GetSignedAsync(signedId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSignedByUserResearchRole(
            [FromBody] GetSignedByUserResearchRoleDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Researcher") && !User.IsInRole("Administrator") && userId != request.UserId)
            {
                return Forbid();
            }

            var result = await _signingService.GetSignedAsync(request.UserId, request.ResearchId, request.Role);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("list")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> GetSignedList([FromQuery] GetSignedListDto request)
        {
            var result = await _signingService.GetSignedListAsync(request.Active, 
                request.UserId, request.ResearchId, request.Role);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("my-list")]
        public async Task<IActionResult> GetMySignedList([FromBody] GetMySignedListDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _signingService.GetSignedListAsync(request.Active, userId, 
                request.ResearchId, request.Role);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
