using DotNetAPISampleApp.Models.IdentityModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DotNetAPISampleApp.Models.ExaminationModels;
using System.Text.Json.Serialization;

namespace DotNetAPISampleApp.Models.ResearchModels
{
    public enum ResearchRole
    {
        Researcher,
        Patient
    }
    public class ResearchSigned
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResearchId { get; set; }

        [JsonIgnore]
        public Research Research { get; set; }

        public string? SignedUserId { get; set; }

        [JsonIgnore]
        public User? SignedUser { get; set; }

        [Required]
        public ResearchRole ResearchRole { get; set; }

        [Required]
        public DateTime ActiveFrom { get; set; }

        public DateTime? ActiveTo { get; set; }

        [JsonIgnore]
        public ICollection<Examination> ResearcherExaminations { get; set; } = new List<Examination>();
        [JsonIgnore]
        public ICollection<Examination> PatientExaminations { get; set; } = new List<Examination>();

    }
}
