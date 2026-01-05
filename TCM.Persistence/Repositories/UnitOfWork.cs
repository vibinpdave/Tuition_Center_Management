namespace TCM.Persistence.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private Dictionary<Type, object> _repositories;
        public DatabaseContext Context { get; }
        public IUserRepository Users { get; }
        public UnitOfWork(DatabaseContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Users = new UserRepository(context);
            //_currentUserService = currentUserService;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IAuditedEntity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            Type type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(Context);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Context?.Dispose();
        }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }
    }
}
