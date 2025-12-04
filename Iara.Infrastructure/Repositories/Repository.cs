using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Iara.Domain.Repositories;
using Iara.Infrastructure.Persistence;

namespace Iara.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation with EF Core
/// </summary>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly IaraDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(IaraDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new[] { id }, cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public virtual async Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}
