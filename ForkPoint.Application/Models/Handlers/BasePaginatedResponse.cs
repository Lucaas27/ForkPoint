// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ForkPoint.Application.Models.Handlers;

public abstract record BasePaginatedResponse<T> : BaseResponse
{
    protected BasePaginatedResponse(IEnumerable<T> items, int totalItemsCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItemsCount = totalItemsCount;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
        ItemsFrom = pageSize * (pageNumber - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }

    public IEnumerable<T> Items { get; init; }
    public int? TotalPages { get; init; }
    public int? TotalItemsCount { get; init; }
    public int? ItemsFrom { get; init; }
    public int? ItemsTo { get; init; }
}