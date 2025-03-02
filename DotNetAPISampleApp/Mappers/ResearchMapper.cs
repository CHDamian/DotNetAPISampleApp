using DotNetAPISampleApp.Models.DTOs.ResearchDTO;
using DotNetAPISampleApp.Models.ResearchModels;

namespace DotNetAPISampleApp.Mappers
{
    public static class ResearchMapper
    {
        public static ResearchDto ToDto(this Research research)
        {
            return new ResearchDto
            {
                Id = research.id,
                Subject = research.subject,
                Summary = research.summary,
                Finished = research.finished,
                ResultsSummary = research.resultsSummary
            };
        }

        public static IEnumerable<ResearchDto> ToDto(this IEnumerable<Research> researchList)
        {
            return researchList.Select(r => r.ToDto());
        }
    }
}
