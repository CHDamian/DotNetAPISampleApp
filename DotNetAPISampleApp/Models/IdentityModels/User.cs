using DotNetAPISampleApp.Models.ResearchModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DotNetAPISampleApp.Models.IdentityModels
{
    public class User : IdentityUser
    {
        [Required]
        [RegularExpression(@"^[0-9]{2}([02468]1|[13579][012])(0[1-9]|1[0-9]|2[0-9]|3[01])[0-9]{5}$",
            ErrorMessage = "Invalid PESEL.")]
        public string PESEL { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [JsonIgnore]
        public ICollection<ResearchSigned> ResearchSigned { get; set; } = new List<ResearchSigned>();
    }
}
