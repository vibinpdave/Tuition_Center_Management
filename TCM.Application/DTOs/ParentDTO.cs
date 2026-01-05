namespace TCM.Application.DTOs
{
    public class ParentDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public string ResidentialAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public bool IsActive { get; set; }
    }
}
