using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grades");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Description)
                .HasMaxLength(250);

            builder.HasMany(g => g.GradeSubjects)
                .WithOne(gs => gs.Grade)
                .HasForeignKey(gs => gs.GradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
