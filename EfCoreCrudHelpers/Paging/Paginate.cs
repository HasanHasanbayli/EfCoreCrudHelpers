namespace EfCoreCrudHelpers.Paging;

public record Paginate<TEntity>
{
    public int From { get; set; }
    public int Index { get; set; }
    public int Size { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public List<TEntity> Items { get; set; } = [];
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
}