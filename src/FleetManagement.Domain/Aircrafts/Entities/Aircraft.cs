using System.ComponentModel.DataAnnotations;
using FleetManagement.Domain.Aircrafts.Events;
using FleetManagement.Domain.CommonEntities;
using FleetManagement.Domain.Enums;

namespace FleetManagement.Domain.Aircrafts.Entities
{
    public class Aircraft : BaseEntity
    {
        // Parameterless constructor required by EF Core
        protected Aircraft() { }

        // Factory method for creating new Aircraft
        public static Aircraft Create(string registrationNumber, string model, AircraftManufacturers manufacturer, AircraftStatus status)
        {
            var aircraft = new Aircraft
            {
                RegistrationNumber = registrationNumber,
                Model = model,
                Manufacturer = manufacturer,
                Status = status,
            };

            // Trigger domain event for new creation
            aircraft.AddDomainEvent(new AircraftCreatedEvent(aircraft.RegistrationNumber));

            return aircraft;
        }


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
