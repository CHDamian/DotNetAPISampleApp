using DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Mappers
{
    public static class ResearchSignedMapper
    {
        public static ResearchSignedDto ToDto(this ResearchSigned researchSigned)
        {
            return new ResearchSignedDto
            {
                Id = researchSigned.Id,
                ResearchId = researchSigned.ResearchId,
                Research = researchSigned.Research.ToDto(),
                SignedUserId = researchSigned.SignedUserId,
                SignedUser = researchSigned.SignedUser?.ToDto(),
                ResearchRole = researchSigned.ResearchRole,
                ActiveFrom = researchSigned.ActiveFrom,
                ActiveTo = researchSigned.ActiveTo
            };
        }

        public static IEnumerable<ResearchSignedDto> ToDto(this IEnumerable<ResearchSigned> researchSignedList)
        {
            return researchSignedList.Select(rs => rs.ToDto());
        }
    }
}
