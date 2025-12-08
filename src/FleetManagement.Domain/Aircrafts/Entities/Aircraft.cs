using FleetManagement.Domain.CommonEntities;
using FleetManagement.Shared.Enums;

namespace FleetManagement.Domain.Aircrafts.Entities
{
    public class Aircraft : BaseEntity
    {
        public required string RegistrationNumber { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public required string Model { get; set; }
        public AircraftManufacturers Manufacturer { get; set; } = AircraftManufacturers.Boeing;
        public int? YearOfManufacture { get; set; }
        public AircraftStatus Status { get; set; } = AircraftStatus.Active;



        // Navigation property to AircraftSpecification
        public AircraftSpecification AircraftSpecification { get; set; } = null!;

    }
}