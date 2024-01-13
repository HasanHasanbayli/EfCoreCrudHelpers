namespace EfCoreCrudHelpers.Dynamic;

public abstract class Filter
{
    public string? Field { get; set; }
    public string? Operator { get; set; }
    public string? Value { get; set; }
    public string? Logic { get; set; }
    public IEnumerable<Filter> Filters { get; set; } = [];
}