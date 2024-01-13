using System.Linq.Dynamic.Core;
using System.Text;
using EfCoreCrudHelpers.Dynamic;
using DynamicModel = EfCoreCrudHelpers.Dynamic.Dynamic;

namespace EfCoreCrudHelpers.Extensions;

public static class QueryableDynamicFilterExtensions
{
    private static readonly string[] Orders = ["asc", "desc"];
    private static readonly string[] Logics = ["and", "or"];

    private static readonly Dictionary<string, string> Operators = new()
    {
        {"eq", "=="},
        {"neq", "!="},
        {"lt", "<"},
        {"lte", "<="},
        {"gt", ">"},
        {"gte", ">="},
        {"isNull", "== null"},
        {"isNotNull", "!= null"},
        {"startsWith", "StartsWith"},
        {"endsWith", "EndsWith"},
        {"contains", "Contains"},
        {"doesNotContain", "Contains"}
    };

    public static IQueryable<T> ToDynamic<T>(
        this IQueryable<T> query, DynamicModel dynamic)
    {
        if (dynamic.Sort.Any())
        {
            query = Sort(query, dynamic.Sort);
        }

        if (dynamic.Filter is not null)
        {
            query = Filter(query, dynamic.Filter);
        }

        return query;
    }

    private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
    {
        List<Filter> filters = GetAllFilters(filter);

        object?[] values = filters.Select(f => f.Value).ToArray();

        string where = Transform(filter, filters);

        if (!string.IsNullOrEmpty(where))
        {
            queryable = queryable.Where(where, values);
        }

        return queryable;
    }

    private static IQueryable<T> Sort<T>(
        IQueryable<T> queryable, IEnumerable<Sort> sorts)
    {
        sorts = sorts.ToList();

        foreach (Sort sort in sorts.ToList())
        {
            if (string.IsNullOrEmpty(sort.Field))
            {
                throw new ArgumentException(message: "Invalid Field");
            }

            if (string.IsNullOrEmpty(sort.Dir) || !Orders.Contains(sort.Dir))
            {
                throw new ArgumentException(message: "Invalid Order Type");
            }
        }

        if (!sorts.Any()) return queryable;

        string ordering = string.Join(",", sorts.Select(sort => $"{sort.Field} {sort.Dir}"));

        return queryable.OrderBy(ordering);
    }

    private static List<Filter> GetAllFilters(Filter filter)
    {
        List<Filter> filters = [];

        GetFilters(filter, filters);

        return filters;
    }

    private static void GetFilters(Filter filter, List<Filter> filters)
    {
        filters.Add(filter);

        if (!filter.Filters.Any()) return;

        foreach (Filter item in filter.Filters)
        {
            GetFilters(item, filters);
        }
    }

    private static string Transform(Filter filter, List<Filter> filters)
    {
        if (string.IsNullOrEmpty(filter.Field))
        {
            throw new ArgumentException(message: "Invalid Field");
        }

        if (string.IsNullOrEmpty(filter.Operator) || !Operators.TryGetValue(filter.Operator, out string? comparison))
        {
            throw new ArgumentException(message: "Invalid Operator");
        }

        StringBuilder where = new();
        int index = filters.IndexOf(filter);

        if (filter.Value is not null)
        {
            if (filter.Operator == "doesNotContain")
            {
                where.Append($"(!np({filter.Field}).{comparison}(@{index}))");
            }
            else if (comparison is "StartsWith" or "EndsWith" or "Contains")
            {
                where.Append($"(np({filter.Field}).{comparison}(@{index}))");
            }
            else
            {
                where.Append($"np({filter.Field}) {comparison} @{index}");
            }
        }
        else if (filter.Operator is "isNull" or "isNotNull")
        {
            where.Append($"np({filter.Field}) {comparison}");
        }

        if (filter.Logic is null || !filter.Filters.Any())
        {
            return where.ToString();
        }

        if (!Logics.Contains(filter.Logic))
        {
            throw new ArgumentException(message: "Invalid Logic");
        }

        return $"{where} {filter.Logic} ({string.Join($" {filter.Logic} ",
            filter.Filters.Select(f => Transform(f, filters)).ToArray())})";
    }
}