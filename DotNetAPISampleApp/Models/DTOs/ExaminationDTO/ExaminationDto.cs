using DotNetAPISampleApp.Models.DTOs.IdentityDTO;
using DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO;
using DotNetAPISampleApp.Models.ExaminationModels;

namespace DotNetAPISampleApp.Models.DTOs.ExaminationDTO
{
    public class ExaminationDto
    {
        public int Id { get; set; }
        public int? ResearcherId { get; set; }
        public ResearchSignedDto? Researcher { get; set; }
        public int? PatientId { get; set; }
        public ResearchSignedDto? Patient { get; set; }
        public DateTime ExamDate { get; set; }
        public ExaminationStatus Status { get; set; }
        public string? Results { get; set; }
    }
}
