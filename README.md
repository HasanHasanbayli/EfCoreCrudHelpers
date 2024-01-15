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

## Getting Started

### Prerequisites

- .NET 8.0

### Installation

1. Add the `EfCore.Crud.Helpers` package to your project.

```bash
dotnet add package EfCore.Crud.Helpers --version 1.0.9
```

## Usage

Here are examples of how to use the `QueryableDynamicFilterExtensions` and `PredicateExtension` classes in your code:

```csharp
using EfCoreCrudHelpers.Dynamic;
using EfCoreCrudHelpers.Extensions;
using System.Linq;

public class SampleUsage
{
    public void UsePredicateExtensionWithAnd(Expression<Func<MyEntity, bool>> expr1, Expression<Func<MyEntity, bool>> expr2)
    {
        var combinedExpr = expr1.And(expr2);
    }

    public void UsePredicateExtensionWithOr(Expression<Func<MyEntity, bool>> expr1, Expression<Func<MyEntity, bool>> expr2)
    {
        var combinedExpr = expr1.Or(expr2);
    }

    public void UsePredicateExtensionWithNot(Expression<Func<MyEntity, bool>> expr)
    {
        var negatedExpr = expr.Not();
    }

    public void UseQueryableDynamicFilterExtensions(IQueryable<MyEntity> query)
    {
        var dynamicModel = new Dynamic
        {
            Sort = new List<Sort>
            {
                new Sort { Field = "Name", Dir = "asc" }
            },
            Filter = new Filter
            {
                Logic = "and",
                Filters = new List<Filter>
                {
                    new Filter { Field = "Age", Operator = "gt", Value = "30" },
                    new Filter { Field = "Country", Operator = "eq", Value = "USA" }
                }
            }
        };

        var result = query.ToDynamic(dynamicModel);
    }
}
```

In this example, `MyEntity` is a placeholder for your actual entity class. Replace it with the class you are querying. The `UseQueryableDynamicFilterExtensions` method demonstrates how to use the `ToDynamic` method from the `QueryableDynamicFilterExtensions` class to apply dynamic sorting and filtering to a query. The `UsePredicateExtension` method demonstrates how to use the `And` method from the `PredicateExtension` class to combine two expressions with a logical AND operator.
