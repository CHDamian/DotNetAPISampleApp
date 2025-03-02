namespace DotNetAPISampleApp.Models.DTOs.IdentityDTO
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PESEL { get; set; }
        public string Role { get; set; }
    }
}
