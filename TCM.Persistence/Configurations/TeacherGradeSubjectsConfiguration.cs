using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class TeacherGradeSubjectsConfiguration : IEntityTypeConfiguration<TeacherGradeSubjects>
    {
        public void Configure(EntityTypeBuilder<TeacherGradeSubjects> builder)
        {
            builder.HasKey(x => x.Id);

            // -------------------------
            // Teacher
            // -------------------------
            builder.HasOne(x => x.Teacher)
                   .WithMany(x => x.TeacherGradeSubjects)
                   .HasForeignKey(x => x.TeacherId)
                   .OnDelete(DeleteBehavior.Cascade);

            // -------------------------
            // GradeSubjects
            // -------------------------
            builder.HasOne(x => x.GradeSubjects)
                   .WithMany()
                   .HasForeignKey(x => x.GradeSubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Unique constraint
            // -------------------------
            builder.HasIndex(x => new { x.TeacherId, x.GradeSubjectId })
                   .IsUnique();
        }
    }
}
