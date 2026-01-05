using TCM.Domain.Common;

namespace TCM.Domain
{
    public class GradeSubjects : AuditedEntity
    {
        public long GradeId { get; set; }
        public long SubjectId { get; set; }

        //Navigation Properties.
        public Grade Grade { get; set; } = default!;
        public Subject Subject { get; set; } = default!;
    }
}
