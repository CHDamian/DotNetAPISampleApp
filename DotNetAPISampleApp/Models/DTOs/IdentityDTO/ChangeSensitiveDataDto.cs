namespace DotNetAPISampleApp.Models.DTOs.IdentityDTO
{
    public class ChangeSensitiveDataDto
    {
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string? PESEL { get; set; }
    }
}
