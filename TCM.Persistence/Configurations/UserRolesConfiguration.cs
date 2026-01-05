using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TCM.Persistence.Configurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(x => x.Guid)
                 .HasDefaultValueSql("NEWID()");

            // Seed default data
            builder.HasData(
                new UserRoles { Id = 1, Name = "Principal", DateCreated = new DateTime(2025, 12, 22, 10, 0, 0), Status = (int)TCM.Domain.Enum.Enums.Status.Active },
                new UserRoles { Id = 2, Name = "Teacher", DateCreated = new DateTime(2025, 12, 22, 10, 0, 0), Status = (int)TCM.Domain.Enum.Enums.Status.Active },
                new UserRoles { Id = 3, Name = "Student", DateCreated = new DateTime(2025, 12, 22, 10, 0, 0), Status = (int)TCM.Domain.Enum.Enums.Status.Active },
                new UserRoles { Id = 4, Name = "Parent", DateCreated = new DateTime(2025, 12, 22, 10, 0, 0), Status = (int)TCM.Domain.Enum.Enums.Status.Active }
            );
        }
    }
}
