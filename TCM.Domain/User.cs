using TCM.Domain.Common;

namespace TCM.Domain
{
    public class User : AuditedEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        // Foreign Key
        public long UserRoleId { get; set; }
        public string Password { get; set; }

        // Navigation Property
        public UserRoles UserRole { get; set; }
    }
}
