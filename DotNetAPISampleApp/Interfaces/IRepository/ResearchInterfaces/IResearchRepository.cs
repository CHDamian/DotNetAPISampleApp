using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces
{
    public interface IResearchRepository
    {
        public Task CreateResearchAsync(Research research);
        public Task UpdateResearchAsync(Research research);
        public Task RemoveResearchByIdAsync(int id);
        public Task<IEnumerable<Research>> GetResearchesAsync();
        public Task<Research?> GetResearchByIdAsync(int id);
    }
}
