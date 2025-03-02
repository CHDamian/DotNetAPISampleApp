using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Interfaces.IService.ExaminationInterfaces;
using DotNetAPISampleApp.Models.DTOs.ExaminationDTO;
using DotNetAPISampleApp.Models.ExaminationModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DotNetAPISampleApp.Controllers.ExaminationControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExaminationController : ControllerBase
    {
        private readonly IExaminationService _examinationService;

        public ExaminationController(IExaminationService examinationService)
        {
            _examinationService = examinationService;
        }

        private string GetLoggedInUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost("add")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> AddExamination([FromBody] AddExaminationRequest request)
        {
            var loggedInId = GetLoggedInUserId();
            var result = await _examinationService.AddExaminationAsync(loggedInId, request.PatientUId, request.ResearchId, request.ExamDate);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("accept/{examinationId}")]
        public async Task<IActionResult> AcceptExamination(int examinationId)
        {
            var loggedInId = GetLoggedInUserId();
            var result = await _examinationService.AcceptExaminationAsync(loggedInId, examinationId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("cancel/{examinationId}")]
        public async Task<IActionResult> CancelExamination(int examinationId)
        {
            var loggedInId = GetLoggedInUserId();
            var result = await _examinationService.CancelExaminationAsync(loggedInId, examinationId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("complete")]
        public async Task<IActionResult> CompleteExamination([FromBody] CompleteExaminationRequest request)
        {
            var loggedInId = GetLoggedInUserId();
            var result = await _examinationService.CompleteExaminationAsync(loggedInId, request.ExaminationId, request.Report);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("reschedule")]
        public async Task<IActionResult> RescheduleExamination([FromBody] RescheduleExaminationRequest request)
        {
            var loggedInId = GetLoggedInUserId();
            var result = await _examinationService.RescheduleExaminationAsync(loggedInId, request.ExaminationId, request.NewExamDate);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("list")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> GetExaminations([FromQuery] int? researcherId, [FromQuery] int? patientId, [FromQuery] ExaminationStatus? status)
        {
            var result = await _examinationService.GetExaminationsAsync(researcherId, patientId, status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("detailed-list")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> GetExaminationsDetailed([FromQuery] string? researcherUId, [FromQuery] string? patientUId, [FromQuery] ExaminationStatus? status)
        {
            var result = await _examinationService.GetExaminationsDetaiedAsync(researcherUId, patientUId, status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("patient-examinations")]
        public async Task<IActionResult> GetPatientExaminations()
        {
            var loggedInId = GetLoggedInUserId();
            var result = await _examinationService.GetExaminationsDetaiedAsync(null, loggedInId, null);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("by-research/{researchId}")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> GetExaminationsByResearch(int researchId)
        {
            var result = await _examinationService.GetExaminationsByResearchAsync(researchId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{examinationId}")]
        [Authorize(Roles = "Researcher,Administrator")]
        public async Task<IActionResult> GetExaminationById(int examinationId)
        {
            var result = await _examinationService.GetExaminationByIdAsync(examinationId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
