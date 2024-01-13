namespace EfCoreCrudHelpers.Dynamic;

public abstract class Dynamic
{
    public IEnumerable<Sort> Sort { get; set; } = [];
    public Filter? Filter { get; set; }
}