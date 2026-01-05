namespace TCM.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IAuditedEntity;
        IUserRepository Users { get; }
        int SaveChanges();

        //Task<int> SaveChangesAsync();
    }
}
