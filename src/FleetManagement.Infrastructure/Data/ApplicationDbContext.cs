using System;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Infrastructure.Data.Interceptors;


namespace FleetManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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

        
    }
}