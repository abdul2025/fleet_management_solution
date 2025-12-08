using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FleetManagement.Domain.Aircrafts.Entities;

namespace FleetManagement.Data.Configurations
{
    public class AircraftSpecificationConfiguration : IEntityTypeConfiguration<AircraftSpecification>
    {
        public void Configure(EntityTypeBuilder<AircraftSpecification> builder)
        {
            // Table name
            builder.ToTable("aircraft_specifications");

            // PK = FK
            builder.HasKey(s => s.AircraftId);

            // BaseStation - Required, Max 100
            builder.Property(s => s.BasedStation)
                   .HasColumnName("based_station")
                   .HasMaxLength(100)
                   .IsRequired();

            // SeatingCapacity
            builder.Property(s => s.SeatingCapacity)
                   .HasColumnName("seating_capacity");

            // MaxTakeoffWeight
            builder.Property(s => s.MaxTakeoffWeight)
                   .HasColumnName("max_takeoff_weight")
                   .HasColumnType("decimal(18,2)");

            // MaxLandingWeight
            builder.Property(s => s.MaxLandingWeight)
                   .HasColumnName("max_landing_weight")
                   .HasColumnType("decimal(18,2)");
            

            builder.Property(s => s.WeightUnit)
                .HasColumnName("weight_unit")
                .HasConversion<int>(); // store enum as int

            // 1:1 relationship with Aircraft
            builder.HasOne(s => s.Aircraft)
                   .WithOne(a => a.AircraftSpecification)
                   .HasForeignKey<AircraftSpecification>(s => s.AircraftId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
