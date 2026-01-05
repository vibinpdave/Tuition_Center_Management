namespace TCM.Application.Contracts.Persistence
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
