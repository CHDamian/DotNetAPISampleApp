using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetAPISampleApp.Models.ResearchModels
{
    public class Research
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string summary { get; set; }

        [Required]
        public bool finished { get; set; }

        public string? resultsSummary { get; set; }

        [JsonIgnore]
        public ICollection<ResearchSigned> ResearchSigned { get; set; } = new List<ResearchSigned>();
    }
}
