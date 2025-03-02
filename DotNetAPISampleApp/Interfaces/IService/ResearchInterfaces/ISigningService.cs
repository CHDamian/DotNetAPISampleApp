using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Interfaces.IService.ResearchInterfaces
{
    public interface ISigningService
    {
        public Task<Result> SignToResearchAsync(string userId, int researchId, ResearchRole role);
        public Task<Result> RemoveFromResearchAsync(int signedId);
        public Task<Result<ResearchSignedDto>> GetSignedAsync(int signedId);
        public Task<Result<ResearchSignedDto>> GetSignedAsync(string userId, int researchId, ResearchRole role);
        public Task<Result<IEnumerable<ResearchSignedDto>>> GetSignedListAsync(bool active, string? userId,
                int? researchId, ResearchRole? role);
    }
}
