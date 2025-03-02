using DotNetAPISampleApp.Models.ResearchModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetAPISampleApp.Models.ExaminationModels
{
    public enum ExaminationStatus
    {
        Requested,
        Confirmed,
        Examined,
        Cancelled
    }
    public class Examination
    {
        [Key]
        public int Id { get; set; }

        public int? ResearcherId { get; set; }
        [JsonIgnore]
        public ResearchSigned? Researcher { get; set; }

        public int? PatientId { get; set; }
        [JsonIgnore]
        public ResearchSigned? Patient { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        [Required]
        public ExaminationStatus Status { get; set; }

        public string? Results { get; set; }
    }
}
