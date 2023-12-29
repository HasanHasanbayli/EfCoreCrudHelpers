using EfCoreCrudHelpers.Paging;
using Microsoft.EntityFrameworkCore;

namespace EfCoreCrudHelpers.Extensions;

public static class PaginateExtension
{
    public static async Task<Paginate<T>> ToPaginateAsync<T>(this IQueryable<T> source, int index, int size,
        int from = 1,
        CancellationToken cancellationToken = default)
    {
        if (from > index) throw new ArgumentException(message: $"From: {from} > Index: {index}, must from <= Index");

        int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

        List<T> items = await source
            .Skip((index - from) * size)
            .Take(size)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        int pages = (int) Math.Ceiling(count / (double) size);

        return new Paginate<T>
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Pages = pages,
            Items = items,
            HasPrevious = index - from > 0,
            HasNext = index - from + 1 < pages
        };
    }

    public static Paginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size,
        int from = 1)
    {
        if (from > index) throw new ArgumentException(message: $"From: {from} > Index: {index}, must from <= Index");

        int count = source.Count();

        List<T> items = source
            .Skip((index - from) * size)
            .Take(size)
            .ToList();

        int pages = (int) Math.Ceiling(count / (double) size);

        return new Paginate<T>
        {
            Index = index,
            Size = size,
            From = from,
            Count = count,
            Pages = pages,
            Items = items,
            HasPrevious = index - from > 0,
            HasNext = index - from + 1 < pages
        };
    }
}