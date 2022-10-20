using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recipes.Application.Abstractions.Data;
using Recipes.Domain.Primitives;
using System.Reflection;
using Recipes.Infrastructure.Extensions;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;

namespace Recipes.Infrastructure
{
    public sealed class ApplicationDbContext : DbContext, IDbContext
    {
        #pragma warning disable CS8618 // Required by Entity Framework
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        public new DbSet<TEntity> Set<TEntity>()
            where TEntity : Entity =>
            base.Set<TEntity>();

        /// <inheritdoc />
        public async Task<TEntity> GetByAsync<TEntity>(Expression<Func<TEntity,
            bool>> expression, CancellationToken cancellationToken = default)
            where TEntity : Entity
        {
            return await Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id,
            CancellationToken cancellationToken = default)
            where TEntity : Entity
        {
            return await Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        /// <inheritdoc />
        public void Insert<TEntity>(TEntity entity)
            where TEntity : Entity =>
            Set<TEntity>().Add(entity);

        /// <inheritdoc />
        public new void Remove<TEntity>(TEntity entity)
            where TEntity : Entity =>
            Set<TEntity>().Remove(entity);

        /// <summary>
        /// Saves all of the pending changes in the unit of work.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The number of entities that have been saved.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();

            return await base.SaveChangesAsync(cancellationToken);
        }

        

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyUtcDateTimeConverter();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Updates the entities implementing <see cref="IAuditableEntity"/> interface.
        /// </summary>
        /// <param name="utcNow">The current date and time in UTC format.</param>
        private void UpdateAuditableEntities()
        {
            foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(nameof(IAuditableEntity.CreatedOnUtc)).CurrentValue = DateTime.UtcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(nameof(IAuditableEntity.ModifiedOnUtc)).CurrentValue = DateTime.UtcNow;
                }
            }
        }
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.BeginTransactionAsync(cancellationToken);
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Database.CommitTransactionAsync(cancellationToken);
        }
    }
}
