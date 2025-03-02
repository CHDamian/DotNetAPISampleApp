using DotNetAPISampleApp.Interfaces.IService.ResearchInterfaces;
using DotNetAPISampleApp.Models.DTOs.ResearchDTO;
using DotNetAPISampleApp.Models.ResearchModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPISampleApp.Controllers.ResearchControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResearchController : ControllerBase
    {
        private readonly IResearchService _researchService;

        public ResearchController(IResearchService researchService)
        {
            _researchService = researchService;
        }

        [HttpPost]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> AddResearch([FromBody] ResearchCreateDto request)
        {
            var result = await _researchService.AddResearchAsync(request.subject, request.summary);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> UpdateResearch(int id, [FromBody] Research request)
        {
            var result = await _researchService.UpdateResearchAsync(id, request.subject, request.summary);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> RemoveResearch(int id)
        {
            var result = await _researchService.RemoveResearchAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> GetAllResearches()
        {
            var result = await _researchService.GetResearchesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResearchById(int id)
        {
            var result = await _researchService.GetResearchByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPatch("{id}/finish")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> SetResearchDone(int id, [FromBody] string resultSummary)
        {
            var result = await _researchService.SetResearchDoneAsync(id, resultSummary);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("{id}/results")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> UpdateResearchResults(int id, [FromBody] string resultSummary)
        {
            var result = await _researchService.UpdateResearchResultsAsync(id, resultSummary);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
