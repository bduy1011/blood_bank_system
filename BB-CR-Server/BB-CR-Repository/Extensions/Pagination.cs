namespace BB.CR.Repositories.Extensions
{
    internal static class Pagination
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int index, int size)
        {
            return query.Skip((index - 1) * size).Take(size);
        }
    }
}
