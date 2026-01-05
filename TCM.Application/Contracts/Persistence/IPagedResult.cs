namespace TCM.Application.Contracts.Persistence
{
    public interface IPagedResult<TEntity>
    {
        public int Count { get; set; }

        public IEnumerable<TEntity> Results { get; set; }
    }
}
