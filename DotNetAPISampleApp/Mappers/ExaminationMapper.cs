using DotNetAPISampleApp.Models.DTOs.ExaminationDTO;
using DotNetAPISampleApp.Models.ExaminationModels;

namespace DotNetAPISampleApp.Mappers
{
    public static class ExaminationMapper
    {
        public static ExaminationDto ToDto(this Examination examination)
        {
            return new ExaminationDto
            {
                Id = examination.Id,
                ResearcherId = examination.ResearcherId,
                Researcher = examination.Researcher?.ToDto(),
                PatientId = examination.PatientId,
                Patient = examination.Patient?.ToDto(),
                ExamDate = examination.ExamDate,
                Status = examination.Status,
                Results = examination.Results
            };
        }

        public static IEnumerable<ExaminationDto> ToDto(this IEnumerable<Examination> examinations)
        {
            return examinations.Select(exam => exam.ToDto());
        }
    }
}
