namespace DotNetAPISampleApp.Models.DTOs.ResearchDTO
{
    public class ResearchDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Summary { get; set; }
        public bool Finished { get; set; }
        public string? ResultsSummary { get; set; }
    }
}
