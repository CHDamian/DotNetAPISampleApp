using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO
{
    public class GetMySignedListDto
    {
        public bool Active { get; set; }
        public int? ResearchId { get; set; }
        public ResearchRole? Role { get; set; }
    }
}
