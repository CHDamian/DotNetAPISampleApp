using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO
{
    public class GetSignedListDto
    {
        public bool Active { get; set; }
        public string? UserId { get; set; }
        public int? ResearchId { get; set; }
        public ResearchRole? Role { get; set; }
    }
}
