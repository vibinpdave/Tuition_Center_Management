using static TCM.Domain.Enum.Enums;

namespace TCM.Application.DTOs
{
    public class StudentDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ResidentialAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string DataFields { get; set; }
        public Status Status { get; set; }

        public ParentDTO Parent { get; set; }
        public GradeDTO Grade { get; set; }
    }
}
