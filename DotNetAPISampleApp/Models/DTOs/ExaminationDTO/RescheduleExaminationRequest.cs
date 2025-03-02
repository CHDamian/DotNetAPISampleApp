namespace DotNetAPISampleApp.Models.DTOs.ExaminationDTO
{
    public class RescheduleExaminationRequest
    {
        public int ExaminationId { get; set; }
        public DateTime NewExamDate { get; set; }
    }
}
