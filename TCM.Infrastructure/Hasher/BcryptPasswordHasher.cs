namespace TCM.Infrastructure.Hasher
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        private readonly int _workFactor;

        public BcryptPasswordHasher(IConfiguration configuration)
        {
            // Read workFactor from config, default to 12 if missing
            _workFactor = configuration.GetValue<int>("Security:BcryptWorkFactor", 12);
        }
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
