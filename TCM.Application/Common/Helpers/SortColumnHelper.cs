namespace TCM.Application.Common.Helpers
{
    public static class SortColumnHelper<TEntity>
    {
        public static string Normalize(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException(ValidationMessages.SORT_COLUMN_IS_REQUIRED);

            var property = typeof(TEntity)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p =>
                    p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

            if (property == null)
                throw new ArgumentException(
                string.Format(ValidationMessages.INVALID_SORT_COLUMN, columnName)
            );

            return property.Name; // exact casing
        }
    }
}
