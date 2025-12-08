using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Infrastructure.Data.Configurations;

namespace FleetManagement.Data.Configurations
{
    public class AircraftConfiguration : BaseEntityConfiguration<Aircraft>
    {
        public override void Configure(EntityTypeBuilder<Aircraft> builder)
        {
            // Call base to configure Id, CreatedAt, UpdatedAt
            base.Configure(builder);

            // Table name
            builder.ToTable("aircrafts");

            // RegistrationNumber - Required, Unique, Max 50 characters
            builder.Property(a => a.RegistrationNumber)
                .HasColumnName("registration_number")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(a => a.RegistrationNumber)
                .IsUnique()
                .HasDatabaseName("ix_aircrafts_registration_number");

            // Model - Required, Max 100 characters
            builder.Property(a => a.Model)
                .HasColumnName("model")
                .HasMaxLength(100)
                .IsRequired();

            // Manufacturer - Convert enum to int, NOT NULL
            builder.Property(a => a.Manufacturer)
                .HasConversion<int>()
                .HasColumnName("manufacturer")
                .HasMaxLength(100)
                .IsRequired();

            // YearOfManufacture - Optional integer
            builder.Property(a => a.YearOfManufacture)
                .HasColumnName("year_of_manufacture")
                .IsRequired(false);

            // Status - Convert enum to int, NOT NULL
            builder.Property(a => a.Status)
                .HasConversion<int>()
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired();

            // Additional indexes
            builder.HasIndex(a => a.Status)
                .HasDatabaseName("ix_aircrafts_status");

            builder.HasIndex(a => a.Model)
                .HasDatabaseName("ix_aircrafts_model");

            builder.HasIndex(a => a.Manufacturer)
                .HasDatabaseName("ix_aircrafts_manufacturer");

            // 1:1 relationship with AircraftSpecification
            builder.HasOne(a => a.AircraftSpecification)
                   .WithOne(s => s.Aircraft)
                   .HasForeignKey<AircraftSpecification>(s => s.AircraftId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
