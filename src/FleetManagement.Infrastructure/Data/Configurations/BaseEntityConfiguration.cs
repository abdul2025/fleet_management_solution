using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FleetManagement.Domain.CommonEntities;

namespace FleetManagement.Infrastructure.Data.Configurations
{
    public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Primary Key - PostgreSQL will create a SERIAL/IDENTITY sequence automatically
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd()  // This tells EF to let the database generate the value
                .IsRequired();

            // CreatedAt
            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            // UpdatedAt
            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);
        }
    }
}