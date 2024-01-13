namespace EfCoreCrudHelpers.Dynamic;

public class Dynamic
{
    public IEnumerable<Sort> Sort { get; set; } = [];
    public Filter? Filter { get; set; }
}