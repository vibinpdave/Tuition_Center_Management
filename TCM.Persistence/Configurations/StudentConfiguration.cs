using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.MobileNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.ResidentialAddress)
                .HasMaxLength(500);

            builder.Property(s => s.City)
                .HasMaxLength(100);

            builder.Property(s => s.State)
                .HasMaxLength(100);

            builder.Property(s => s.Country)
                .HasMaxLength(100);

            // -------------------------
            // Student → Parent (N:1)
            // -------------------------
            builder.HasOne(s => s.Parent)
                .WithMany(p => p.Students)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Student → Grade (N:1)
            // -------------------------
            builder.HasOne(s => s.Grade)
                .WithMany()
                .HasForeignKey(s => s.GradeId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Student → User (1:1)
            // -------------------------
            builder.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Indexes
            // -------------------------
            builder.HasIndex(s => s.Email).IsUnique();
            builder.HasIndex(s => s.MobileNumber);
            builder.HasIndex(s => s.GradeId);
            builder.HasIndex(s => s.ParentId);
        }
    }
}
