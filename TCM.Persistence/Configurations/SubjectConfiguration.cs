using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Description)
                .HasMaxLength(250);

            // Relationship
            builder.HasMany(s => s.GradeSubjects)
                .WithOne(gs => gs.Subject)
                .HasForeignKey(gs => gs.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
