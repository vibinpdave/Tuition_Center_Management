namespace TCM.Persistence.Repositories
{
    public class PagedResult<TEntity> : IPagedResult<TEntity>
    {
        public int Count { get; set; }

        public IEnumerable<TEntity> Results { get; set; }
    }
}
