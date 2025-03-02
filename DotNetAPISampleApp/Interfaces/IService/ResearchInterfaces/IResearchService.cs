using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Interfaces.IService.ResearchInterfaces
{
    public interface IResearchService
    {
        public Task<Result> AddResearchAsync(string subject, string summary);
        public Task<Result> UpdateResearchAsync(int id, string? subject, string? summary);
        public Task<Result> RemoveResearchAsync(int id);
        public Task<Result<IEnumerable<Research>>> GetResearchesAsync();
        public Task<Result<Research>> GetResearchByIdAsync(int id);
        public Task<Result> SetResearchDoneAsync(int id, string resultSummary);
        public Task<Result> UpdateResearchResultsAsync(int id, string resultSummary);
    }
}
