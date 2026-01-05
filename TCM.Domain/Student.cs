using TCM.Domain.Common;

namespace TCM.Domain
{
    public class Student : AuditedEntity
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ResidentialAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        // --------------------
        // Grade (Many → One)
        // --------------------
        public long GradeId { get; set; }
        public Grade Grade { get; set; }

        // --------------------
        // Parent (Many → One)
        // --------------------
        public long ParentId { get; set; }
        public Parent Parent { get; set; }

        // Login
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
