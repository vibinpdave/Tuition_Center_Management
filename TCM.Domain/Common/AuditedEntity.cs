namespace TCM.Domain.Common
{
    public class AuditedEntity : IAuditedEntity
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public int? Status { get; set; }
    }
}
