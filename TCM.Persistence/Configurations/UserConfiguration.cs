using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            // Relationship configuration
            builder.HasOne(u => u.UserRole)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.UserRoleId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
