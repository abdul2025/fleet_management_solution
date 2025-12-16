using FleetManagement.Shared.Enums.AircraftEnums;

namespace FleetManagement.Application.Aircrafts.DTOs
{
    public class AircraftSpecificationDto
    {
        public string BasedStation { get; set; } = string.Empty;
        public int SeatingCapacity { get; set; }
        public decimal MaxTakeoffWeight { get; set; }
        public decimal MaxLandingWeight { get; set; }
        public WeightUnit WeightUnit { get; set; }
    }
}
