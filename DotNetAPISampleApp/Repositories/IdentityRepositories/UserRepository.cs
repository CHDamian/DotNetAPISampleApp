using DotNetAPISampleApp.Data;
using DotNetAPISampleApp.Interfaces.IRepository.IdentityInterfaces;
using DotNetAPISampleApp.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DotNetAPISampleApp.Repositories.IdentityRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<User> CreateUserAsync(User user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return null;

            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return user;
        }
        public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _userManager.FindByNameAsync(usernameOrEmail) ??
                   await _userManager.FindByEmailAsync(usernameOrEmail);
        }
        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task UpdateUserPersonalDataAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
        public async Task UpdateUserSensitiveDataAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
        public async Task SetUserRoleAsync(User user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                throw new Exception("Role does not exist.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<bool> CheckIfUserIsInRole(User user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
    }
}
