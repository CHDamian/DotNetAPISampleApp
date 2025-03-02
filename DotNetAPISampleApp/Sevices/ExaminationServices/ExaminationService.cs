using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Interfaces.IRepository.ExaminationInterfaces;
using DotNetAPISampleApp.Interfaces.IRepository.IdentityInterfaces;
using DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces;
using DotNetAPISampleApp.Interfaces.IService.ExaminationInterfaces;
using DotNetAPISampleApp.Mappers;
using DotNetAPISampleApp.Models.DTOs.ExaminationDTO;
using DotNetAPISampleApp.Models.ExaminationModels;
using DotNetAPISampleApp.Models.IdentityModels;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Sevices.ExaminationServices
{
    public class ExaminationService : IExaminationService
    {
        private readonly IExaminationRepository _examinationRepository;
        private readonly IResearchSignedRepository _researchSignedRepository;
        private readonly IResearchRepository _researchRepository;
        private readonly IUserRepository _userRepository;

        public ExaminationService(IExaminationRepository examinationRepository, 
            IResearchSignedRepository researchSignedRepository, IResearchRepository researchRepository, 
            IUserRepository userRepository)
        {
            _examinationRepository = examinationRepository;
            _researchSignedRepository = researchSignedRepository;
            _researchRepository = researchRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> AddExaminationAsync(string researcherUId, string patientUId,
            int researchId, DateTime examDate)
        {
            try
            {
                var researcher = await _researchSignedRepository.GetActiveSignAsync(researcherUId, 
                    researchId, ResearchRole.Researcher);
                var patient = await _researchSignedRepository.GetActiveSignAsync(patientUId,
                    researchId, ResearchRole.Patient);

                if (researcher == null || patient == null)
                    return Result.Fail("Active researcher or patient not found");

                var examination = new Examination
                {
                    Researcher = researcher,
                    Patient = patient,
                    ExamDate = examDate,
                    Status = ExaminationStatus.Requested,
                    Results = null
                };

                await _examinationRepository.AddExaminationAsync(examination);
                return Result.Ok("Examination requested!");

            }
            catch(Exception e)
            {
                return Result.Fail("An error occurred while preparing examination.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result> AcceptExaminationAsync(string loggedInId, int examinationId)
        {
            try
            {
                var examination = await _examinationRepository.GetExaminationById(examinationId);
                if(examination == null)
                    return Result.Fail("Examination not found");
                if(examination.Patient == null || examination.Patient.SignedUser == null 
                    || examination.Patient.SignedUser.Id != loggedInId)
                    return Result.Fail("You can't accept this examination.");
                if(examination.Status != ExaminationStatus.Requested)
                    return Result.Fail("You can't accept this examination.");

                examination.Status = ExaminationStatus.Confirmed;
                await _examinationRepository.UpdateExaminationAsync(examination);
                return Result.Ok("Examination accepted!");

            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while accepting examination.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result> CancelExaminationAsync(string loggedInId, int examinationId)
        {
            try
            {
                var examination = await _examinationRepository.GetExaminationById(examinationId);
                if (examination == null)
                    return Result.Fail("Examination not found");
                Result? resultForbid = null;
                if (examination.Patient == null || examination.Patient.SignedUser == null
                    || examination.Patient.SignedUser.Id != loggedInId)
                    resultForbid = Result.Fail("You can't cancel this examination.");

                if (resultForbid != null && (examination.Researcher == null
                    || examination.Researcher.SignedUser == null
                    || examination.Researcher.SignedUser.Id != loggedInId))
                    return resultForbid;

                if (examination.Status == ExaminationStatus.Examined)
                    return Result.Fail("You can't cancel this examination.");


                examination.Status = ExaminationStatus.Cancelled;
                await _examinationRepository.UpdateExaminationAsync(examination);
                return Result.Ok("Examination cancelled!");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while canceling examination.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result> CompleteExaminationAsync(string loggedInId, int examinationId, string report)
        {
            try
            {
                var examination = await _examinationRepository.GetExaminationById(examinationId);
                if (examination == null)
                    return Result.Fail("Examination not found");
                if (examination.Researcher == null || examination.Researcher.SignedUser == null
                    || examination.Researcher.SignedUser.Id != loggedInId)
                    return Result.Fail("You can't complete this examination.");
                if (examination.Status != ExaminationStatus.Confirmed)
                    return Result.Fail("You can't complete this examination.");

                examination.Status = ExaminationStatus.Examined;
                examination.Results = report;
                await _examinationRepository.UpdateExaminationAsync(examination);
                return Result.Ok("Examination completed!");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while finishing examination.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result> RescheduleExaminationAsync(string loggedInId, int examinationId, 
            DateTime newExamDate)
        {
            try
            {
                var examination = await _examinationRepository.GetExaminationById(examinationId);
                if (examination == null)
                    return Result.Fail("Examination not found");
                if (examination.Researcher == null || examination.Researcher.SignedUser == null
                    || examination.Researcher.SignedUser.Id != loggedInId)
                    return Result.Fail("You can't reschedule this examination.");
                if (examination.Status == ExaminationStatus.Examined)
                    return Result.Fail("You can't reschedule this examination.");

                examination.ExamDate = newExamDate;
                await _examinationRepository.UpdateExaminationAsync(examination);
                return Result.Ok("Examination rescheduled!");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while rescheduling examination.",
                    new List<string> { e.Message });
            }
        }

        public async Task<Result> SetExaminationStatusAsync(int examinationId, ExaminationStatus status, 
            string? report)
        {
            try
            {
                var examination = await _examinationRepository.GetExaminationById(examinationId);
                if (examination == null)
                    return Result.Fail("Examination not found");

                if(status == ExaminationStatus.Examined && report == null)
                    return Result.Fail("Examination need to have report to set Examined");

                if (status != ExaminationStatus.Examined && report != null)
                    return Result.Fail("Examination can't have report when not Examined");

                examination.Status = status;
                examination.Results = report;
                await _examinationRepository.UpdateExaminationAsync(examination);
                return Result.Ok("Examination rescheduled!");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while rescheduling examination.",
                    new List<string> { e.Message });
            }
        }

        public async Task<Result> ChangeExaminationResultsAsync(string loggedInId, int examinationId, string report)
        {
            try
            {
                var examination = await _examinationRepository.GetExaminationById(examinationId);
                if (examination == null)
                    return Result.Fail("Examination not found");

                if (examination.Status != ExaminationStatus.Examined)
                    return Result.Fail("Examination need to be Examined to set results");

                examination.Results = report;
                await _examinationRepository.UpdateExaminationAsync(examination);
                return Result.Ok("Examination rescheduled!");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while rescheduling examination.",
                    new List<string> { e.Message });
            }
        }

        public async Task<Result<IEnumerable<ExaminationDto>>> GetExaminationsAsync(int? researcherId, 
            int? patientId, ExaminationStatus? status)
        {
            try
            {
                return Result<IEnumerable<ExaminationDto>>.Ok((await _examinationRepository
                    .GetExaminationsAsync(researcherId, patientId, status)).ToDto());
            }
            catch (Exception e)
            {
                return Result<IEnumerable<ExaminationDto>>.Fail("An error occurred while fetching examinations.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result<IEnumerable<ExaminationDto>>> GetExaminationsDetaiedAsync(string? researcherUId,
            string? patientUId, ExaminationStatus? status)

        {
            try
            {
                return Result<IEnumerable<ExaminationDto>>.Ok((await _examinationRepository
                    .GetExaminationsDetailedAsync(researcherUId, patientUId, status)).ToDto());
            }
            catch (Exception e)
            {
                return Result<IEnumerable<ExaminationDto>>.Fail("An error occurred while fetching examinations.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result<IEnumerable<ExaminationDto>>> GetExaminationsByResearchAsync(int researchId)
        {
            try
            {
                return Result<IEnumerable<ExaminationDto>>.Ok((await _examinationRepository
                    .GetExaminationsByReaserchAsync(researchId)).ToDto());
            }
            catch (Exception e)
            {
                return Result<IEnumerable<ExaminationDto>>.Fail("An error occurred while fetching examinations.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result<ExaminationDto>> GetExaminationByIdAsync(int examinationId)
        {
            try
            {
                return Result<ExaminationDto>.Ok((await _examinationRepository
                    .GetExaminationById(examinationId)).ToDto());
            }
            catch (Exception e)
            {
                return Result<ExaminationDto>.Fail("An error occurred while fetching examination.",
                    new List<string> { e.Message });
            }
        }

    }
}
