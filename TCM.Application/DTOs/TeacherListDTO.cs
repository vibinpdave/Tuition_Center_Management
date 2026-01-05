namespace TCM.Application.DTOs
{
    public class TeacherListDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Qualification { get; set; }
        public bool IsActive { get; set; }
    }
}
