namespace BuildingBlocks.Pagination;
public record PaginatedResult<TResult>(int PageIndex, int PageSize, long Count, IEnumerable<TResult> Data)
	where TResult : class;
