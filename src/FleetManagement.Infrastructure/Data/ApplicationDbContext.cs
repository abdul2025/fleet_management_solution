using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Infrastructure.Data.Interceptors;
using FleetManagement.Application.Aircrafts.Interfaces;
using FleetManagement.Domain.CommonEntities;
using System.Threading;
using System.Threading.Tasks;

namespace FleetManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly DomainEventDispatcher _dispatcher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    DomainEventDispatcher dispatcher) // <-- inject here
            : base(options)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        // DbSets
        public DbSet<Aircraft> Aircrafts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(new BaseEntityInterceptor());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var result = base.SaveChanges();

            _dispatcher.DispatchAll(entitiesWithEvents).GetAwaiter().GetResult(); // call async properly

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            await _dispatcher.DispatchAll(entitiesWithEvents);

            return result;
        }
    }
}
