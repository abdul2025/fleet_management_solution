using System.ComponentModel.DataAnnotations;
using FleetManagement.Shared.Enums.AircraftEnums;

namespace FleetManagement.Domain.Aircrafts.Entities
{
    public class AircraftSpecification
    {
        [Key]
        public int AircraftId { get; set; }

        [Required, StringLength(50)]
        public required string BasedStation { get; set; }

        [Range(1, 600)]
        public int SeatingCapacity { get; set; }

        [Range(1, 1000000)]
        public decimal MaxTakeoffWeight { get; set; }

        [Range(1, 1000000)]
        public decimal MaxLandingWeight { get; set; }

        [Required]
        public WeightUnit WeightUnit { get; set; }


        // Navigation property
        public Aircraft Aircraft { get; set; } = null!;
    }
}
