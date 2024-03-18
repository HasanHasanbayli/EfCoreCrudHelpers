# EF Core CRUD Helpers

EF Core CRUD Helpers is a library designed to simplify CRUD operations using Entity Framework Core. It offers convenient helper methods through classes such as `QueryableDynamicFilterExtensions` and `PredicateExtension`, providing extension methods for `IQueryable<T>` and `Expression<Func<T, bool>>` respectively.

## Features

- **Pagination:** Extension methods to paginate `IQueryable` sources.
- **Dynamic Filtering and Sorting:** The `QueryableDynamicFilterExtensions` class facilitates dynamic sorting and filtering on `IQueryable<T>` objects.
- **Expression Combination:** The `PredicateExtension` class combines expressions with logical operators.

## Classes

### QueryableDynamicFilterExtensions

The `QueryableDynamicFilterExtensions` is a static class that extends `IQueryable<T>`. These methods are employed to apply dynamic sorting and filtering to an `IQueryable<T>` object.

#### Methods

- `ToDynamic<T>(this IQueryable<T> query, DynamicModel dynamic)`: Applies sorting and filtering to the `IQueryable<T>` object based on rules defined in the `DynamicModel` object.
- `Filter<T>(IQueryable<T> queryable, Filter filter)`: Applies a filter to the `IQueryable<T>` object based on rules defined in the `Filter` object.
- `Sort<T>(IQueryable<T> queryable, List<Sort> sort)`: Applies sorting rules to the `IQueryable<T>` object based on rules defined in the `Sort` object.

### PredicateExtension

The `PredicateExtension` is a static class that provides extension methods for `Expression<Func<T, bool>>`. These methods combine expressions with logical operators.

#### Methods

- `And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)`: Combines two expressions with a logical AND operator.
- `Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)`: Combines two expressions with a logical OR operator.
- `Not<T>(this Expression<Func<T, bool>> expr)`: Negates an expression with a logical NOT operator.

## Support
If you find this project helpful or just want to support my work, consider buying me a coffee. Your support is greatly appreciated!

<a href="https://www.buymeacoffee.com/hasanhasanbayli" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" width="200"  ></a>

## Getting Started

### Prerequisites

- .NET 8.0

### Installation

1. Add the `EfCore.Crud.Helpers` package to your project.

```bash
dotnet add package EfCore.Crud.Helpers --version 1.1.2
```

## Usage

Here are examples of how to use the `QueryableDynamicFilterExtensions` and `PredicateExtension` classes in your code:

```csharp
using System.Linq.Expressions;
using EfCoreCrudHelpers.Extensions;

public class SampleClass
{
    private int Id { get; init; }
    public string Name { get; init; }

    private readonly List<SampleClass> _list =
    [
        new SampleClass {Id = 1, Name = "Name1"},
        new SampleClass {Id = 2, Name = "Name2"},
        new SampleClass {Id = 3, Name = "Name3"},
        new SampleClass {Id = 4, Name = "Name4"},
        new SampleClass {Id = 5, Name = "Name5"}
    ];

    public void UsePredicateExtensions()
    {
        Expression<Func<SampleClass, bool>> predicate = sampleClass => sampleClass.Id == 1;
        Expression<Func<SampleClass, bool>> predicate2 = sampleClass => sampleClass.Id == 2;
        Expression<Func<SampleClass, bool>> predicate3 = sampleClass => sampleClass.Id == 3;

        Expression<Func<SampleClass, bool>> combinedPredicateWithAnd = predicate.And(predicate2);
        Expression<Func<SampleClass, bool>> combinedPredicateWithOr = predicate.Or(predicate2);
        Expression<Func<SampleClass, bool>> combinedPredicateWithNot = predicate3.Not();

        IQueryable<SampleClass> result = _list.AsQueryable()
            .Where(combinedPredicateWithAnd.And(combinedPredicateWithOr).Or(combinedPredicateWithNot));
    }

    public void UseQueryableDynamicFilterExtensions()
    {
        Dynamic dynamicModel = new()
        {
            Sort = new List<Sort>
            {
                new() {Field = "Name", Dir = "asc"}
            },
            Filter = new Filter
            {
                Field = "Country",
                Operator = "eq",
                Value = "USA",

                Logic = "and",

                Filters = new List<Filter>
                {
                    new() {Field = "Age", Operator = "gt", Value = "30"}
                }
            }
        };

        IQueryable<SampleClass> result = _list.AsQueryable().ToDynamic(dynamicModel);
    }
}
```

In this example, `MyEntity` is a placeholder for your actual entity class. Replace it with the class you are querying. The `UseQueryableDynamicFilterExtensions` method demonstrates how to use the `ToDynamic` method from the `QueryableDynamicFilterExtensions` class to apply dynamic sorting and filtering to a query. The `UsePredicateExtension` method demonstrates how to use the `And` method from the `PredicateExtension` class to combine two expressions with a logical AND operator.