namespace TCM.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Set<User>()
                .Include(u => u.UserRole)              // include role if needed
                .Where(u => u.Status == (int)Enums.Status.Active)
                .FirstOrDefaultAsync(u => u.Email == username);
        }
    }
}
