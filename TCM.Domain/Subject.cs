using TCM.Domain.Common;

namespace TCM.Domain
{
    public class Subject : AuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        //Navigation Properties.
        public ICollection<GradeSubjects> GradeSubjects { get; set; }
    }
}
