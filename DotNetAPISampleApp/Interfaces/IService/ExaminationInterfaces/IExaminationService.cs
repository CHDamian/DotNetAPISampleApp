using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Models.DTOs.ExaminationDTO;
using DotNetAPISampleApp.Models.ExaminationModels;

namespace DotNetAPISampleApp.Interfaces.IService.ExaminationInterfaces
{
    public interface IExaminationService
    {
        public Task<Result> AddExaminationAsync(string researcherUId, string patientUId,
            int researchId, DateTime examDate);
        public Task<Result> AcceptExaminationAsync(string loggedInId, int examinationId);
        public Task<Result> CancelExaminationAsync(string loggedInId, int examinationId);
        public Task<Result> CompleteExaminationAsync(string loggedInId, int examinationId, string report);
        public Task<Result> RescheduleExaminationAsync(string loggedInId, int examinationId, DateTime newExamDate);
        public Task<Result> SetExaminationStatusAsync(int examinationId, ExaminationStatus status, string? report);
        public Task<Result> ChangeExaminationResultsAsync(string loggedInId, int examinationId, string report);
        public Task<Result<IEnumerable<ExaminationDto>>> GetExaminationsAsync(int? researcherId,
            int? patientId, ExaminationStatus? status);
        public Task<Result<IEnumerable<ExaminationDto>>> GetExaminationsDetaiedAsync(string? researcherUId,
            string? patientUId, ExaminationStatus? status);
        public Task<Result<IEnumerable<ExaminationDto>>> GetExaminationsByResearchAsync(int researchId);
        public Task<Result<ExaminationDto>> GetExaminationByIdAsync(int examinationId);
    }
}
