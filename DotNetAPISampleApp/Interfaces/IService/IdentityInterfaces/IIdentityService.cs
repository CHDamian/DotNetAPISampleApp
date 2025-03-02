using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Models.DTOs.IdentityDTO;
using DotNetAPISampleApp.Models.IdentityModels;

namespace DotNetAPISampleApp.Interfaces.IService.IdentityInterfaces
{
    public interface IIdentityService
    {
        public Task<Result<User>> RegisterUserAsync(string email, string username, string password,
            string name, string surname, string PESEL, string role);
        public Task<Result<User>> LoginUserAsync(string usernameOrEmail, string password);
        public Task<Result> LogoutUserAsync();
        public Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        public Task<Result> ChangePersonalDataAsync(string userId, string? name, string? surname);
        public Task<Result> ChangeSensitiveDataAsync(string userId, string? email, string? PESEL);
        public Task<Result> SetUserRoleAsync(string userId, string role);
        public Task<Result<List<UserDto>>> GetAllUsersAsync();

    }
}
