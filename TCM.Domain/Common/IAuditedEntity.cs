using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TCM.Domain.Common
{
    public interface IAuditedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public int? Status { get; set; }
    }
}
