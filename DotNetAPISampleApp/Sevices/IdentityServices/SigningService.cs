using DotNetAPISampleApp.Common;
using DotNetAPISampleApp.Interfaces.IRepository.IdentityInterfaces;
using DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces;
using DotNetAPISampleApp.Interfaces.IService.ResearchInterfaces;
using DotNetAPISampleApp.Mappers;
using DotNetAPISampleApp.Models.DTOs.ResearchSignedDTO;
using DotNetAPISampleApp.Models.IdentityModels;
using DotNetAPISampleApp.Models.ResearchModels;
using System.Collections.Generic;

namespace DotNetAPISampleApp.Sevices.IdentityServices
{
    public class SigningService : ISigningService
    {
        private readonly IUserRepository            _userRepository;
        private readonly IResearchRepository        _researchRepository;
        private readonly IResearchSignedRepository  _researchSignedRepository;

        public SigningService(IUserRepository userRepository, 
            IResearchRepository researchRepository, IResearchSignedRepository researchSignedRepository)
        {
            _userRepository = userRepository;
            _researchRepository = researchRepository;
            _researchSignedRepository = researchSignedRepository;
        }
        public async Task<Result> SignToResearchAsync(string userId, int researchId, ResearchRole role)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userId);
                var research = await _researchRepository.GetResearchByIdAsync(researchId);
                if (user == null || research == null)
                    return Result.Fail("User or research not found.");

                if( role == ResearchRole.Researcher
                    && !await _userRepository.CheckIfUserIsInRole(user, "Researcher")
                    && !await _userRepository.CheckIfUserIsInRole(user, "Administrator"))
                    return Result.Fail("User cannot be researcher.");

                var oldSigns = await _researchSignedRepository.GetAllSignedAsync(true, userId, researchId, role);
                if(oldSigns.Any())
                    return Result.Fail("User is already signed.");

                var sign = new ResearchSigned
                {
                    SignedUser = user,
                    Research = research,
                    ResearchRole = role,
                    ActiveFrom = DateTime.Now,
                    ActiveTo = null
                };
                await _researchSignedRepository.SignUserToResearchAsync(sign);
                return Result.Ok("User signed to research!");
            }
            catch(Exception e)
            {
                return Result.Fail("An error occurred while signing the user.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result> RemoveFromResearchAsync(int signedId)
        {
            try
            {
                var signed = await _researchSignedRepository.GetSignByIdAsync(signedId);
                if (signed.ActiveTo != null)
                    return Result.Fail("User was already removed from research.");

                await _researchSignedRepository.RemoveUserFromResearchAsync(signed);
                return Result.Ok("User successfuly removed from research.");
            }
            catch(Exception e)
            {
                return Result.Fail("An error occurred while removing the user from research.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result<ResearchSignedDto>> GetSignedAsync(int signedId)
        {
            try
            {
                var sign = await _researchSignedRepository.GetSignByIdAsync(signedId);
                if (sign == null)
                    return Result<ResearchSignedDto>.Fail("Sign not found.");
                return Result<ResearchSignedDto>.Ok(sign.ToDto(), "Sign succesfully found.");
            }
            catch(Exception e)
            {
                return Result<ResearchSignedDto>.Fail("An error occurred while searching for sign.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result<ResearchSignedDto>> GetSignedAsync(string userId, int researchId, ResearchRole role)
        {
            try
            {
                var sign = await _researchSignedRepository.GetActiveSignAsync(userId, researchId, role);
                if (sign == null)
                    return Result<ResearchSignedDto>.Fail("Sign not found.");
                return Result<ResearchSignedDto>.Ok(sign.ToDto(), "Sign succesfully found.");
            }
            catch (Exception e)
            {
                return Result<ResearchSignedDto>.Fail("An error occurred while searching for sign.",
                    new List<string> { e.Message });
            }
        }
        public async Task<Result<IEnumerable<ResearchSignedDto>>> GetSignedListAsync(bool active, string? userId,
                int? researchId, ResearchRole? role)
        {
            try
            {
                var sign = await _researchSignedRepository.GetAllSignedAsync(active, userId, researchId, role);
                if (sign == null)
                    return Result<IEnumerable<ResearchSignedDto>>.Fail("Sign not found.");
                return Result<IEnumerable<ResearchSignedDto>>.Ok(sign.ToDto(), "Sign succesfully found.");
            }
            catch (Exception e)
            {
                return Result<IEnumerable<ResearchSignedDto>>.Fail("An error occurred while searching for sign.",
                    new List<string> { e.Message });
            }
        }
    }
}
