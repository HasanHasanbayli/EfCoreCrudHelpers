namespace EfCoreCrudHelpers.Dynamic;

public class Filter
{
    public string? Field { get; set; }
    public string? Operator { get; set; }
    public object? Value { get; set; }
    public string? Logic { get; set; }
    public IEnumerable<Filter> Filters { get; set; } = [];
}