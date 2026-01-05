namespace TCM.Application.DTOs
{
    public class AccessTokenResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public long UserRoleId { get; set; }

        public TokenDetailsDTO TokenDetails { get; set; }
    };

    public class TokenDetailsDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
