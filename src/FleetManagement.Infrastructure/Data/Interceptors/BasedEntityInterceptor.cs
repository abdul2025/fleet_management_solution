using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FleetManagement.Domain.CommonEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FleetManagement.Infrastructure.Data.Interceptors
{
    /// <summary>
    /// Interceptor that automatically manages CreatedAt and UpdatedAt timestamps for BaseEntity.
    /// - CreatedAt: Set only once when entity is added (if not already set)
    /// - UpdatedAt: Set when entity is added (if not already set), and always updated on modifications
    /// </summary>
    public class BaseEntityInterceptor : SaveChangesInterceptor
    {
        /// <summary>
        /// Intercepts synchronous SaveChanges calls
        /// </summary>
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateTimestamps(eventData.Context);
            return result;
        }

        /// <summary>
        /// Intercepts asynchronous SaveChangesAsync calls
        /// </summary>
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateTimestamps(eventData.Context);
            return new ValueTask<InterceptionResult<int>>(result);
        }

        /// <summary>
        /// Updates timestamps for entities being added or modified
        /// </summary>
        private void UpdateTimestamps(DbContext context)
        {
            if (context == null) return;

            // Get all BaseEntity entries that are being added or modified
            var entries = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    // Only set CreatedAt if it hasn't been set manually
                    if (entry.Entity.CreatedAt == default)
                    {
                        entry.Entity.CreatedAt = now;
                    }
                    
                    // Only set UpdatedAt if it hasn't been set manually
                    if (entry.Entity.UpdatedAt == default)
                    {
                        entry.Entity.UpdatedAt = now;
                    }
                }
                else if (entry.State == EntityState.Modified)
                {
                    // ALWAYS update UpdatedAt on modifications to reflect the actual last update time
                    entry.Entity.UpdatedAt = now;
                }
            }
        }
    }
}