using DotNetAPISampleApp.Models.IdentityModels;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces
{
    public interface IResearchSignedRepository
    {
        public Task SignUserToResearchAsync(ResearchSigned researchSigned);
        public Task RemoveUserFromResearchAsync(ResearchSigned researchSigned);
        public Task<ResearchSigned> GetSignByIdAsync(int id);
        public Task<ResearchSigned> GetActiveSignAsync(string userId,
                int researchId, ResearchRole role);
        public Task<IEnumerable<ResearchSigned>> GetAllSignedAsync(bool active, string? userId,
                int? researchId, ResearchRole? role);
    }
}
