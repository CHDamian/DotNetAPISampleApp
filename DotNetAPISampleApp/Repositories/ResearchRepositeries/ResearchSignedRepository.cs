using DotNetAPISampleApp.Data;
using DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces;
using DotNetAPISampleApp.Models.ResearchModels;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPISampleApp.Repositories.ResearchRepositeries
{
    public class ResearchSignedRepository : IResearchSignedRepository
    {
        private readonly ApplicationDbContext _context;

        public ResearchSignedRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SignUserToResearchAsync(ResearchSigned researchSigned)
        {
            _context.ResearchSigneds.Add(researchSigned);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveUserFromResearchAsync(ResearchSigned researchSigned)
        {
            if (researchSigned.ActiveTo != null)
                throw new Exception("User was no longer active!");
            researchSigned.ActiveTo = DateTime.Now;
            _context.ResearchSigneds.Update(researchSigned);
            await _context.SaveChangesAsync();
        }

        public async Task<ResearchSigned> GetSignByIdAsync(int id)
        {
            return await _context.ResearchSigneds
                .Include(rs => rs.Research)
                .Include(rs => rs.SignedUser)
                .FirstOrDefaultAsync(rs => rs.Id == id);
        }

        public async Task<ResearchSigned> GetActiveSignAsync(string userId,
                int researchId, ResearchRole role)
        {
            return await _context.ResearchSigneds.FirstOrDefaultAsync(
                rs => rs.SignedUserId == userId
                && rs.ResearchId == researchId
                && rs.ResearchRole == role);
        }

        public async Task<IEnumerable<ResearchSigned>> GetAllSignedAsync(
            bool active,
            string? userId = null,
            int? researchId = null,
            ResearchRole? role = null)
        {
            IQueryable<ResearchSigned> query = _context.ResearchSigneds
                .Include(rs => rs.Research)
                .Include(rs => rs.SignedUser);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(rs => rs.SignedUserId == userId);

            if (researchId.HasValue)
                query = query.Where(rs => rs.ResearchId == researchId.Value);

            if (role.HasValue)
                query = query.Where(rs => rs.ResearchRole == role.Value);

            if (active)
                query = query.Where(rs => rs.ActiveTo == null);

            return await query.ToListAsync();
        }
    }
}
