using TCM.Domain.Common;

namespace TCM.Domain
{
    public class Parent : AuditedEntity
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ResidentialAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        // Navigation
        public ICollection<Student> Students { get; set; } = new List<Student>();

        // Login
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
