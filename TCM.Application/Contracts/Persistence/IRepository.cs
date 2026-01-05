using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace TCM.Application.Contracts.Persistence
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        IPagedResult<TEntity> GetAllPaged(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageSize = 20, int pageIndex = 0);

        TEntity Single(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        TEntity GetByID(object id);

        Task<TEntity> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[] includes);

        TEntity GetByGUID(Guid id);

        void Insert(TEntity entity);

        void SetAsDeleted(Guid id);

        void SetAsDeleted(long id);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        Task DeleteAsync(TEntity entity);

        void Update(TEntity entityToUpdate);
        DbSet<TEntity> GetEntity();
    }
}
