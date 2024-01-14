using System.Linq.Expressions;
using EfCoreCrudHelpers.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using DynamicModel = EfCoreCrudHelpers.Dynamic.Dynamic;

namespace EfCoreCrudHelpers.Extensions;

public static class EfCoreExtension
{
    public static TEntity? Get<TEntity>(this DbSet<TEntity> entity,
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = true) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable();

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        return queryable.FirstOrDefault(predicate);
    }

    public static List<TEntity> GetList<TEntity>(this DbSet<TEntity> entity,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int size = default,
        bool enableTracking = true) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable();

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        if (predicate != null) queryable = queryable.Where(predicate);

        if (size != default) queryable = queryable.Take(size);

        if (orderBy != null) queryable = orderBy(queryable);

        return queryable.ToList();
    }

    public static Paginate<TEntity> GetPaginatedList<TEntity>(this DbSet<TEntity> entity,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool enableTracking = true) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable();

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        if (predicate != null) queryable = queryable.Where(predicate);

        if (orderBy != null) queryable = orderBy(queryable);

        return queryable.ToPaginate(index, size, from: 1);
    }

    public static List<TEntity> GetDynamicList<TEntity>(this DbSet<TEntity> entity,
        DynamicModel dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int size = default,
        bool enableTracking = true) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable().ToDynamic(dynamic);

        if (predicate != null) queryable = queryable.Where(predicate);

        if (size != default) queryable = queryable.Take(size);

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        return queryable.ToList();
    }

    public static Paginate<TEntity> GetPaginatedDynamicList<TEntity>(this DbSet<TEntity> entity,
        DynamicModel dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool enableTracking = true) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable().ToDynamic(dynamic);

        if (predicate != null) queryable = queryable.Where(predicate);

        if (size != default) queryable = queryable.Take(size);

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        return queryable.ToPaginate(index, size, from: 1);
    }

    public static async Task<TEntity?> GetAsync<TEntity>(this DbSet<TEntity> entity,
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable();

        if (include != null) queryable = include(queryable);

        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public static async Task<List<TEntity>> GetListAsync<TEntity>(this DbSet<TEntity> entity,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int size = default,
        bool enableTracking = true,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable();

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        if (predicate != null) queryable = queryable.Where(predicate);

        if (size != default) queryable = queryable.Take(size);

        if (orderBy != null) queryable = orderBy(queryable);

        return await queryable.ToListAsync(cancellationToken);
    }

    public static async Task<List<TEntity>> GetDynamicListAsync<TEntity>(this DbSet<TEntity> entity,
        DynamicModel dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int size = default,
        bool enableTracking = true,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable().ToDynamic(dynamic);

        if (predicate != null) queryable = queryable.Where(predicate);
        
        if (size != default) queryable = queryable.Take(size);

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        return await queryable.ToListAsync(cancellationToken);
    }

    public static async Task<Paginate<TEntity>> GetPaginatedListAsync<TEntity>(this DbSet<TEntity> entity,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool enableTracking = true,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable();

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        if (predicate != null) queryable = queryable.Where(predicate);

        if (orderBy != null) queryable = orderBy(queryable);

        return await queryable.ToPaginateAsync(index, size, from: 1, cancellationToken);
    }

    public static Task<Paginate<TEntity>> GetPaginatedDynamicListAsync<TEntity>(this DbSet<TEntity> entity,
        DynamicModel dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 1,
        int size = 10,
        bool enableTracking = true) where TEntity : class
    {
        IQueryable<TEntity> queryable = entity.AsQueryable().ToDynamic(dynamic);

        if (predicate != null) queryable = queryable.Where(predicate);

        if (!enableTracking) queryable = queryable.AsNoTracking();

        if (include != null) queryable = include(queryable);

        return queryable.ToPaginateAsync(index, size, from: 1);
    }

    public static TEntity AddEntity<TEntity>(this DbContext context,
        TEntity entity)
        where TEntity : class
    {
        context.Set<TEntity>().Add(entity);

        context.SaveChanges();

        return entity;
    }

    public static async Task<TEntity> AddEntityAsync<TEntity>(this DbContext context,
        TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        await context.Set<TEntity>().AddAsync(entity, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public static List<TEntity> AddEntityRange<TEntity>(this DbContext context,
        List<TEntity> entities)
        where TEntity : class
    {
        context.Set<TEntity>().AddRange(entities);

        context.SaveChanges();

        return entities;
    }

    public static async Task<List<TEntity>> AddEntityRangeAsync<TEntity>(this DbContext context,
        List<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return entities;
    }

    public static TEntity UpdateEntity<TEntity>(this DbContext context,
        TEntity entity)
        where TEntity : class
    {
        context.Set<TEntity>().Update(entity);

        context.SaveChanges();

        return entity;
    }

    public static async Task<TEntity> UpdateEntityAsync<TEntity>(this DbContext context,
        TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().Update(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public static List<TEntity> UpdateEntityRange<TEntity>(this DbContext context,
        List<TEntity> entities)
        where TEntity : class
    {
        context.Set<TEntity>().UpdateRange(entities);

        context.SaveChanges();

        return entities;
    }

    public static async Task<List<TEntity>> UpdateEntityRangeAsync<TEntity>(this DbContext context,
        List<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().UpdateRange(entities);

        await context.SaveChangesAsync(cancellationToken);

        return entities;
    }

    public static TEntity DeleteEntity<TEntity>(this DbContext context,
        TEntity entity)
        where TEntity : class
    {
        context.Set<TEntity>().Remove(entity);

        context.SaveChanges();

        return entity;
    }

    public static async Task<TEntity> DeleteEntityAsync<TEntity>(this DbContext context,
        TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public static List<TEntity> DeleteEntityRange<TEntity>(this DbContext context,
        List<TEntity> entities)
        where TEntity : class
    {
        context.Set<TEntity>().RemoveRange(entities);

        context.SaveChanges();

        return entities;
    }

    public static async Task<List<TEntity>> DeleteEntityRangeAsync<TEntity>(this DbContext context,
        List<TEntity> entities, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        context.Set<TEntity>().RemoveRange(entities);

        await context.SaveChangesAsync(cancellationToken);

        return entities;
    }
}