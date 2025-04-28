namespace BookHive.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> queryable, int page, int pageSize)
    {
        return queryable.Skip((page - 1) * pageSize).Take(pageSize);
    }
    public static int TotalPages(this int totalCount, int pageSize=12)
    {
        return (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
