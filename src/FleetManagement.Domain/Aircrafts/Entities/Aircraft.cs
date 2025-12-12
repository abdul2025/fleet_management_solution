using System.ComponentModel.DataAnnotations;
using FleetManagement.Domain.CommonEntities;
using FleetManagement.Shared.Enums;

namespace FleetManagement.Domain.Aircrafts.Entities
{
    public class Aircraft : BaseEntity
    {
        [Required, StringLength(12)]
        public required string RegistrationNumber { get; set; }

        [StringLength(50)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public required string Model { get; set; }

        [Required]
        public AircraftManufacturers Manufacturer { get; set; }

        [Range(1950, 2050)]
        public int? YearOfManufacture { get; set; }

        [Required]
        public AircraftStatus Status { get; set; }

        public AircraftSpecification AircraftSpecification { get; set; } = null!;
    }
}
