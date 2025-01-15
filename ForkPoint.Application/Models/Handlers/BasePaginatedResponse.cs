// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ForkPoint.Application.Models.Handlers;

public abstract record BasePaginatedResponse<T> : BaseHandlerResponse
{
    protected BasePaginatedResponse(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItemsCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }

    public IEnumerable<T> Items { get; init; }
    public int TotalPages { get; init; }
    public int TotalItemsCount { get; init; }
    public int ItemsFrom { get; init; }
    public int ItemsTo { get; init; }
}