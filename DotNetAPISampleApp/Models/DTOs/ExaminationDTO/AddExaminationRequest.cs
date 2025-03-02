namespace DotNetAPISampleApp.Models.DTOs.ExaminationDTO
{
    public class AddExaminationRequest
    {
        public string PatientUId { get; set; }
        public int ResearchId { get; set; }
        public DateTime ExamDate { get; set; }
    }
}
