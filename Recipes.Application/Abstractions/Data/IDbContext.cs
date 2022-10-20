using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Recipes.Domain.Primitives;
using System.Linq.Expressions;

namespace Recipes.Application.Abstractions.Data;

public interface IDbContext
{
    DatabaseFacade Database { get;  }
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets the database set for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The database set for the specified entity type.</returns>
    DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity;

    /// <summary>
    /// Gets the entity with the specified identifier.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="expression">The entity expression.</param>
    /// <returns>The maybe instance that may contain the <typeparamref name="TEntity"/> with the specified identifier.</returns>
    Task<TEntity> GetByAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        where TEntity : Entity;

    /// <summary>
    /// Gets the entity with the specified identifier.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="id">The entity identifier.</param>
    /// <returns>The maybe instance that may contain the <typeparamref name="TEntity"/> with the specified identifier.</returns>
    Task<TEntity> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default)
        where TEntity : Entity;

    /// <summary>
    /// Inserts the specified entity into the database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be inserted into the database.</param>
    void Insert<TEntity>(TEntity entity)
        where TEntity : Entity;

    /// <summary>
    /// Removes the specified entity from the database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be removed from the database.</param>
    void Remove<TEntity>(TEntity entity)
        where TEntity : Entity;

    /// <summary>
    /// Saves all of the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of entities that have been saved.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
