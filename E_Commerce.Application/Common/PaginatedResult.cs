namespace E_Commerce.Application.Common;

public sealed class PaginatedResult<TEntity>(int pageIndex, int pageSize, int totalCount, IReadOnlyList<TEntity> data)
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public int Count { get; } = totalCount;
    public IReadOnlyList<TEntity> Data { get; } = data;
}
