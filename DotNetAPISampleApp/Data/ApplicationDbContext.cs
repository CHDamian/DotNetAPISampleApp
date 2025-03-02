using DotNetAPISampleApp.Models.ExaminationModels;
using DotNetAPISampleApp.Models.IdentityModels;
using DotNetAPISampleApp.Models.ResearchModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPISampleApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Research> Researches { get; set; }
        public DbSet<ResearchSigned> ResearchSigneds { get; set; }
        public DbSet<Examination> Examinations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.PESEL)
                .IsUnique();

            builder.Entity<ResearchSigned>()
                .HasOne(rs => rs.Research)
                .WithMany(r => r.ResearchSigned)
                .HasForeignKey(rs => rs.ResearchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ResearchSigned>()
                .HasOne(rs => rs.SignedUser)
                .WithMany(u => u.ResearchSigned)
                .HasForeignKey(rs => rs.SignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ResearchSigned>()
                .Property(rs => rs.ResearchRole)
                .HasConversion<string>()
                .IsRequired();

            builder.Entity<Examination>()
                .HasOne(e => e.Researcher)
                .WithMany(rs => rs.ResearcherExaminations)
                .HasForeignKey(e => e.ResearcherId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Examination>()
                .HasOne(e => e.Patient)
                .WithMany(rs => rs.PatientExaminations)
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.SetNull);
        }


    }
}
