using DotNetAPISampleApp.Models.IdentityModels;
using System.Data;

namespace DotNetAPISampleApp.Interfaces.IRepository.IdentityInterfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(User user, string password, string role);
        public Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
        public Task<User> GetUserByIdAsync(string id);
        public Task UpdateUserPersonalDataAsync(User user);
        public Task UpdateUserSensitiveDataAsync(User user);
        public Task SetUserRoleAsync(User user, string role);
        public Task<bool> CheckIfUserIsInRole(User user, string role);
        public Task<List<User>> GetAllUsersAsync();
    }
}
