using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces;
using DotNetAPISampleApp.Interfaces.IService.ResearchInterfaces;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Sevices.ResearchSevices
{
    public class ResearchService : IResearchService
    {
        private readonly IResearchRepository _researchRepository;

        public ResearchService(IResearchRepository researchRepository)
        {
            _researchRepository = researchRepository;
        }

        public async Task<Result> AddResearchAsync(string subject, string summary)
        {
            var research = new Research
            {
                subject = subject,
                summary = summary,
                finished = false,
                resultsSummary = null
            };

            await _researchRepository.CreateResearchAsync(research);
            return Result.Ok("Research added successfully.");
        }

        public async Task<Result> UpdateResearchAsync(int id, string? subject, string? summary)
        {
            var research = await _researchRepository.GetResearchByIdAsync(id);
            if (research == null)
                return Result.Fail("Research not found.");

            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(summary))
                return Result.Fail("At least one field must be provided.");

            if (!string.IsNullOrWhiteSpace(subject))
                research.subject = subject;

            if (!string.IsNullOrWhiteSpace(summary))
                research.summary = summary;

            await _researchRepository.UpdateResearchAsync(research);
            return Result.Ok("Research updated successfully.");
        }

        public async Task<Result> RemoveResearchAsync(int id)
        {
            try
            {
                await _researchRepository.RemoveResearchByIdAsync(id);
            }
            catch(Exception e)
            {
                return Result.Fail("An error occured while deleting record.", new List<string> { e.Message });
            }
            return Result.Ok("Research deleted successfully.");
        }

        public async Task<Result<IEnumerable<Research>>> GetResearchesAsync()
        {
            var researches = await _researchRepository.GetResearchesAsync();
            return Result<IEnumerable<Research>>.Ok(researches);
        }

        public async Task<Result<Research>> GetResearchByIdAsync(int id)
        {
            var research = await _researchRepository.GetResearchByIdAsync(id);
            return research != null ? Result<Research>.Ok(research) : Result<Research>.Fail("Research not found.");
        }

        public async Task<Result> SetResearchDoneAsync(int id, string resultSummary)
        {
            var research = await _researchRepository.GetResearchByIdAsync(id);
            if(research == null)
                return Result.Fail("Research not found.");
            if(research.finished)
                return Result.Fail("Research is already finished.");
            research.finished = true;
            research.resultsSummary = resultSummary;

            await _researchRepository.UpdateResearchAsync(research);
            return Result.Ok("Research progression saved.");
        }
        public async Task<Result> UpdateResearchResultsAsync(int id, string resultSummary)
        {
            var research = await _researchRepository.GetResearchByIdAsync(id);
            if (research == null)
                return Result.Fail("Research not found.");
            if (!research.finished)
                return Result.Fail("Research is not finished yet.");
            research.resultsSummary = resultSummary;

            await _researchRepository.UpdateResearchAsync(research);
            return Result.Ok("Research progression saved.");
        }


    }
}
