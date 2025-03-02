using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO
{
    public class GetSignedByUserResearchRoleDto
    {
        public string UserId { get; set; }
        public int ResearchId { get; set; }
        public ResearchRole Role { get; set; }
    }
}
