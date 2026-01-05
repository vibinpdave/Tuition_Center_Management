namespace TCM.Application.DTOs
{
    public class TeacherDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ResidentialAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Qualification { get; set; }

        public List<TeacherGradeSubjectDto> GradeSubjects { get; set; } = new();
    }
}
