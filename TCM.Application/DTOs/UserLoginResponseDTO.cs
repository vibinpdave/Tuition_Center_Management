namespace TCM.Application.DTOs
{
    public class UserLoginResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public long UserRoleId { get; set; }
    }
}
