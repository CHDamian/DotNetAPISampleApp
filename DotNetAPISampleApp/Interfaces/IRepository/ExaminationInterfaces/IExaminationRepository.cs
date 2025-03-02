using DotNetAPISampleApp.Models.ExaminationModels;

namespace DotNetAPISampleApp.Interfaces.IRepository.ExaminationInterfaces
{
    public interface IExaminationRepository
    {
        public Task AddExaminationAsync(Examination examination);
        public Task UpdateExaminationAsync(Examination examination);
        public Task<IEnumerable<Examination>> GetExaminationsAsync(int? researcherId, int? patientId,
            ExaminationStatus? status);
        public Task<IEnumerable<Examination>> GetExaminationsDetailedAsync(string? researcherUId, 
            string? patientUId, ExaminationStatus? status);
        public Task<IEnumerable<Examination>> GetExaminationsByReaserchAsync(int researchId);
        public Task<Examination?> GetExaminationById(int id);
    }
}
