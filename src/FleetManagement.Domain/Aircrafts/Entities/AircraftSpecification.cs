using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Shared.Enums.AircraftEnums;

namespace FleetManagement.Domain.Aircrafts.Entities
{
    public class AircraftSpecification
    {
        // Primary Key & Foreign Key to Aircraft
        public int AircraftId { get; set; }

        // Base station (airport or city where aircraft is based)
        public required string BasedStation { get; set; }

        // Operational specifications
        public int SeatingCapacity { get; set; } = 0;
        public decimal MaxTakeoffWeight { get; set; } = 0;
        public decimal MaxLandingWeight { get; set; } = 0;

        public WeightUnit WeightUnit { get; set; } = WeightUnit.Kg;


        

        // Navigation property
        public Aircraft Aircraft { get; set; } = null!;
    }
}
