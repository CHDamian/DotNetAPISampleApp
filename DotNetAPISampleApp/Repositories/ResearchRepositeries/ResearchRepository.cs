using DotNetAPISampleApp.Data;
using DotNetAPISampleApp.Interfaces.IRepository.ResearchInterfaces;
using DotNetAPISampleApp.Models.IdentityModels;
using DotNetAPISampleApp.Models.ResearchModels;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPISampleApp.Repositories.ResearchRepositeries
{
    public class ResearchRepository : IResearchRepository
    {
        private readonly ApplicationDbContext _context;

        public ResearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateResearchAsync(Research research)
        {
            _context.Researches.Add(research);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateResearchAsync(Research research)
        {
            _context.Researches.Update(research);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveResearchByIdAsync(int id)
        {
            var research = await _context.Researches.FindAsync(id);
            if (research == null)
            {
                throw new Exception("Id not found!");
            }
            _context.Researches.Remove(research);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Research>> GetResearchesAsync()
        {
            return await _context.Researches.ToListAsync();
        }
        public async Task<Research?> GetResearchByIdAsync(int id)
        {
            return await _context.Researches.FindAsync(id);
        }
    }
}
