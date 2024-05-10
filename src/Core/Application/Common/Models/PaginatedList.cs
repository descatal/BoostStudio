using Microsoft.EntityFrameworkCore;

namespace BoostStudio.Application.Common.Models;

public class PaginatedList<T>(IReadOnlyCollection<T> items, int totalCount, int pageNumber, int pageSize)
{
    public IReadOnlyCollection<T> Items { get; } = items;
    
    public int PageNumber { get; } = pageNumber;
    
    public int TotalPages { get; } = (int)Math.Ceiling(totalCount / (double)pageSize);
    
    public int TotalCount { get; } = totalCount;

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
