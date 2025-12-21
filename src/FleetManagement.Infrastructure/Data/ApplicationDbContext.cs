using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Infrastructure.Data.Interceptors;
using FleetManagement.Application.Aircrafts.Interfaces;
using FleetManagement.Domain.CommonEntities;


namespace FleetManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        private readonly DomainEventDispatcher _dispatcher = new DomainEventDispatcher();


        // DbSets
        public DbSet<Aircraft> Aircrafts { get; set; }


        // Override OnConfiguring to add the interceptor, for any Model creation or modification
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Register the interceptor
            optionsBuilder.AddInterceptors(new BaseEntityInterceptor());
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }



        public override int SaveChanges()
        {
            // Find entities with domain events
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var result = base.SaveChanges();

            // Dispatch domain events after saving
            _dispatcher.DispatchAll(entitiesWithEvents);

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

            // Dispatch domain events after saving
            _dispatcher.DispatchAll(entitiesWithEvents);

            return result;
        }

        
    }
}