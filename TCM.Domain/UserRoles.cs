using TCM.Domain.Common;

namespace TCM.Domain
{
    public class UserRoles : AuditedEntity
    {
        public string Name { get; set; }

        // Optional: navigation back to users
        public ICollection<User> Users { get; set; }
    }
}
