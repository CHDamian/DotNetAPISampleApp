using DotNetAPISampleApp.Models.DTOs.IdentityDTO;
using DotNetAPISampleApp.Models.IdentityModels;

namespace DotNetAPISampleApp.Mappers
{
    public static class UserMapper
    {
        public static UserComponentDto ToDto(this User user)
        {
            return new UserComponentDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                PESEL = user.PESEL,
            };
        }

        public static IEnumerable<UserComponentDto> ToDto(this IEnumerable<User> userList)
        {
            return userList.Select(u => u.ToDto());
        }
    }
}
