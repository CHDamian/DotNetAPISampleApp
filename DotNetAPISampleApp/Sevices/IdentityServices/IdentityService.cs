using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Interfaces.IRepository.IdentityInterfaces;
using DotNetAPISampleApp.Interfaces.IService.IdentityInterfaces;
using DotNetAPISampleApp.Models.DTOs.IdentityDTO;
using DotNetAPISampleApp.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace DotNetAPISampleApp.Sevices.IdentityServices
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public IdentityService(IUserRepository userRepository, SignInManager<User> signInManager, 
            UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Result<User>> RegisterUserAsync(string email, string username, string password, 
            string name, string surname, string PESEL, string role)
        {
            try
            {
                var user = new User
                {
                    Email = email,
                    UserName = username,
                    Name = name,
                    Surname = surname,
                    PESEL = PESEL
                };

                user = await _userRepository.CreateUserAsync(user, password, role);
                if (user == null)
                {
                    return Result<User>.Fail("User creation failed.");
                }

                return Result<User>.Ok(user);
            }
            catch (Exception e)
            {
                return Result<User>.Fail("An error occurred while registering the user.",
                    new List<string> { e.Message });
            }
        }

        public async Task<Result<User>> LoginUserAsync(string usernameOrEmail, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameOrEmailAsync(usernameOrEmail);
                if (user == null)
                {
                    return Result<User>.Fail("User not found.");
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    return Result<User>.Ok(user);
                }

                return Result<User>.Fail("Invalid login attempt");
            }
            catch (Exception e)
            {
                return Result<User>.Fail("An error occurred during login.", new List<string> { e.Message });
            }
        }

        public async Task<Result> LogoutUserAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Result.Ok("Logout successful");
            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred during logout.", new List<string> { ex.Message });
            }
        }

        public async Task<Result> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Result.Fail("User not found");
                }

                var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (result.Succeeded)
                {
                    return Result.Ok("Password changed successfully");
                }

                return Result.Fail("Failed to change password", result.Errors.Select(e => e.Description).ToList());
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred during password change", new List<string> { e.Message });
            }
        }

        public async Task<Result> ChangePersonalDataAsync(string userId, string? name, string? surname)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return Result.Fail("User not found.");

                if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(surname))
                    return Result.Fail("At least one field must be provided.");
                if (!string.IsNullOrEmpty(name)) 
                    user.Name = name;
                if (!string.IsNullOrEmpty(surname)) 
                    user.Surname = surname;

                await _userRepository.UpdateUserPersonalDataAsync(user);
                return Result.Ok("Personal data updated successfully.");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while updating personal data.", new List<string> { e.Message });
            }
        }

        public async Task<Result> ChangeSensitiveDataAsync(string userId, string? email, string? PESEL)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return Result.Fail("User not found.");

                if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(PESEL))
                    return Result.Fail("At least one field must be provided.");
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                if (!string.IsNullOrEmpty(PESEL))
                    user.PESEL = PESEL;

                await _userRepository.UpdateUserSensitiveDataAsync(user);
                return Result.Ok("Personal data updated successfully.");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while updating personal data.", new List<string> { e.Message });
            }
        }
        public async Task<Result> SetUserRoleAsync(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Result.Fail("User not found.");
                }

                await _userRepository.SetUserRoleAsync(user, role);
                return Result.Ok("Role updated successfully.");
            }
            catch (Exception e)
            {
                return Result.Fail("An error occurred while updating role.", new List<string> { e.Message });
            }
        }

        public async Task<Result<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                var userList = new List<UserDto>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userList.Add(new UserDto
                    {
                        Id = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Name = user.Name,
                        Surname = user.Surname,
                        PESEL = user.PESEL,
                        Role = roles.FirstOrDefault() ?? "Unknown"
                    });
                }

                return Result<List<UserDto>>.Ok(userList);
            }
            catch(Exception e)
            {
                return Result<List<UserDto>>.Fail("An error occurred while getting users.",
                    new List<string> { e.Message });
            }
        }
    }
}
