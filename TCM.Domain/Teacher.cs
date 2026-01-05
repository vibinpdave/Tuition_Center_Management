using TCM.Domain.Common;

namespace TCM.Domain
{
    public class Teacher : AuditedEntity
    {
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ResidentialAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Qualification { get; set; }

        // Teaching assignments
        public ICollection<TeacherGradeSubjects> TeacherGradeSubjects { get; set; }
            = new List<TeacherGradeSubjects>();

        // Login mapping
        public long UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
