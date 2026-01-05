using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class GradeSubjectConfiguration : IEntityTypeConfiguration<GradeSubjects>
    {
        public void Configure(EntityTypeBuilder<GradeSubjects> builder)
        {
            builder.HasKey(gs => gs.Id);

            builder.HasOne(gs => gs.Grade)
                .WithMany(g => g.GradeSubjects)
                .HasForeignKey(gs => gs.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gs => gs.Subject)
                .WithMany(s => s.GradeSubjects)
                .HasForeignKey(gs => gs.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate subject assignment per grade
            builder.HasIndex(gs => new { gs.GradeId, gs.SubjectId })
                .IsUnique();
        }
    }
}
