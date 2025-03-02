namespace DotNetAPISampleApp.Models.DTOs.IdentityDTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PESEL { get; set; }
        public string Role { get; set; }
    }
}
