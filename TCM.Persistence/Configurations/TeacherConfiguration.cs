using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(x => x.MobileNumber)
                   .IsRequired()
                   .HasMaxLength(15);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.ResidentialAddress)
                   .HasMaxLength(500);

            builder.Property(x => x.City)
                   .HasMaxLength(100);

            builder.Property(x => x.State)
                   .HasMaxLength(100);

            builder.Property(x => x.Country)
                   .HasMaxLength(100);

            builder.Property(x => x.Qualification)
                   .HasMaxLength(200);

            // -------------------------
            // Teacher → User (1–1)
            // -------------------------
            builder.HasOne(x => x.User)
                   .WithOne()
                   .HasForeignKey<Teacher>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Teacher → TeacherGradeSubjects (1–Many)
            // -------------------------
            builder.HasMany(x => x.TeacherGradeSubjects)
                   .WithOne(x => x.Teacher)
                   .HasForeignKey(x => x.TeacherId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
