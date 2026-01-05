namespace TCM.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IAuditedEntity
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;
        //private readonly long _currentUser;

        public Repository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
            //_currentUser = currentUser;
        }

        public IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (include != null)
            {
                query = include(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query.ToList();
            }
        }

        public IPagedResult<TEntity> GetAllPaged(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageSize = 20, int pageIndex = 1)
        {
            IQueryable<TEntity> query = dbSet;
            var pagedResult = new PagedResult<TEntity>();
            if (include != null)
            {
                query = include(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            pagedResult.Results = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            pagedResult.Count = query.Count();

            return pagedResult;
        }

        public TEntity Single(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (include != null)
            {
                query = include(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.FirstOrDefault();
        }

        public DbSet<TEntity> GetEntity()
        {
            return dbSet;
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public virtual TEntity GetByGUID(Guid id)
        {
            IQueryable<TEntity> query = dbSet;
            return query.FirstOrDefault(x => x.Guid == id);
        }

        public virtual void SetAsDeleted(Guid id)
        {
            IQueryable<TEntity> query = dbSet;
            var entityToUpdate = query.FirstOrDefault(x => x.Guid == id);
            entityToUpdate.Status = (int)Enums.Status.Deleted;
            // entityToUpdate.DeletedBy = _currentUser;
            entityToUpdate.DateModified = DateTime.UtcNow;
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }
        public virtual void SetAsDeleted(long id)
        {
            IQueryable<TEntity> query = dbSet;
            var entityToUpdate = query.FirstOrDefault(x => x.Id == id);
            entityToUpdate.Status = (int)Enums.Status.Deleted;
            entityToUpdate.DateModified = DateTime.UtcNow;
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }
        public virtual void Insert(TEntity entity)
        {
            if (entity is IAuditedEntity auditableEntity)
            {
                auditableEntity.DateCreated = DateTime.UtcNow;
                auditableEntity.DateModified = DateTime.UtcNow;
                auditableEntity.Status = (int)Enums.Status.Active;
                auditableEntity.Guid = Guid.NewGuid();
            }

            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);

            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            //entityToDelete.DeletedBy = _currentUser;
            entityToDelete.DateModified = DateTime.UtcNow;
            dbSet.Remove(entityToDelete);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }


        public virtual void Update(TEntity entityToUpdate)
        {
            //entityToUpdate.UpdatedBy = _currentUser;
            entityToUpdate.DateModified = DateTime.UtcNow;
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

    }
}
