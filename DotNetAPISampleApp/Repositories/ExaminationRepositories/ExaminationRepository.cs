using DotNetAPISampleApp.Data;
using DotNetAPISampleApp.Interfaces.IRepository.ExaminationInterfaces;
using DotNetAPISampleApp.Models.ExaminationModels;
using DotNetAPISampleApp.Models.ResearchModels;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPISampleApp.Repositories.ExaminationRepositories
{
    public class ExaminationRepository : IExaminationRepository
    {
        private readonly ApplicationDbContext _context;

        public ExaminationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddExaminationAsync(Examination examination)
        {
            _context.Examinations.Add(examination);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateExaminationAsync(Examination examination)
        {
            _context.Examinations.Update(examination);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Examination>> GetExaminationsAsync(int? researcherId, 
            int? patientId, ExaminationStatus? status)
        {
            IQueryable<Examination> query = _context.Examinations
                .AsNoTracking()
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.SignedUser)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.SignedUser)
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.Research)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.Research);

            if (researcherId.HasValue)
            {
                query = query.Where(e => e.ResearcherId == researcherId.Value);
            }

            if (patientId.HasValue)
            {
                query = query.Where(e => e.PatientId == patientId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Examination>> GetExaminationsDetailedAsync(string? researcherUId, string? patientUId, ExaminationStatus? status)
        {
            IQueryable<Examination> query = _context.Examinations
                .AsNoTracking()
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.SignedUser)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.SignedUser)
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.Research)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.Research);

            if (!string.IsNullOrEmpty(researcherUId))
            {
                query = query.Where(e => e.Researcher != null && e.Researcher.SignedUserId == researcherUId);
            }

            if (!string.IsNullOrEmpty(patientUId))
            {
                query = query.Where(e => e.Patient != null && e.Patient.SignedUserId == patientUId);
            }

            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Examination>> GetExaminationsByReaserchAsync(int researchId)
        {
            IQueryable<Examination> query = _context.Examinations
                .AsNoTracking()
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.SignedUser)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.SignedUser)
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.Research)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.Research);

            query = query.Where(e => (e.Researcher != null && e.Researcher.ResearchId == researchId) ||
                                     (e.Patient != null && e.Patient.ResearchId == researchId));

            return await query.ToListAsync();
        }

        public async Task<Examination?> GetExaminationById(int id)
        {
            return await _context.Examinations
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.SignedUser)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.SignedUser)
                .Include(e => e.Researcher)
                    .ThenInclude(r => r.Research)
                .Include(e => e.Patient)
                    .ThenInclude(p => p.Research)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}
