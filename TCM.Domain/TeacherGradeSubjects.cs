using TCM.Domain.Common;

namespace TCM.Domain
{
    public class TeacherGradeSubjects : AuditedEntity
    {
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; } = default!;

        public long GradeSubjectId { get; set; }
        public GradeSubjects GradeSubjects { get; set; } = default!;
    }
}
