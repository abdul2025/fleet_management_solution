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

            // Manufacturer - Optional, Max 100 characters
            builder.Property(a => a.Manufacturer)
                .HasColumnName("manufacturer")
                .HasMaxLength(100)
                .IsRequired(false);

            // YearOfManufacture - Optional integer
            builder.Property(a => a.YearOfManufacture)
                .HasColumnName("year_of_manufacture")
                .IsRequired(false);

            // Status - Optional, Max 50 characters
            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired(false);

            // Additional Indexes (optional but recommended)
            builder.HasIndex(a => a.Status)
                .HasDatabaseName("ix_aircrafts_status");

            builder.HasIndex(a => a.Model)
                .HasDatabaseName("ix_aircrafts_model");


        }
    }
}