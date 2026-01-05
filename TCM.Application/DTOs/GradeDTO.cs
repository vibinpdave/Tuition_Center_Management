namespace TCM.Application.DTOs
{
    public class GradeDTO
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
    }
}
