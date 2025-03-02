using DotNetAPISampleApp.Models.DTOs.IdentityDTO;
using DotNetAPISampleApp.Models.DTOs.ResearchDTO;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO
{
    public class ResearchSignedDto
    {
        public int Id { get; set; }
        public int ResearchId { get; set; }
        public ResearchDto Research { get; set; }
        public string? SignedUserId { get; set; }
        public UserComponentDto? SignedUser { get; set; }
        public ResearchRole ResearchRole { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
    }
}
