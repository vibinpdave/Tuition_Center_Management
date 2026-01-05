using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class ParentConfiguration : IEntityTypeConfiguration<Parent>
    {
        public void Configure(EntityTypeBuilder<Parent> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

            builder.Property(p => p.MobileNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.ResidentialAddress)
                .HasMaxLength(500);

            builder.Property(p => p.City)
                .HasMaxLength(100);

            builder.Property(p => p.State)
                .HasMaxLength(100);

            builder.Property(p => p.Country)
                .HasMaxLength(100);

            // -------------------------
            // Parent → Students (1:N)
            // -------------------------
            builder.HasMany(p => p.Students)
                .WithOne(s => s.Parent)
                .HasForeignKey(s => s.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Parent → User (1:1)
            // -------------------------
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------------
            // Indexes
            // -------------------------
            builder.HasIndex(p => p.Email).IsUnique();
            builder.HasIndex(p => p.MobileNumber);
        }
    }
}
