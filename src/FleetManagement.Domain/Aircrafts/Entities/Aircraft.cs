using FleetManagement.Domain.CommonEntities;

namespace FleetManagement.Domain.Aircrafts.Entities
{
    public class Aircraft : BaseEntity
    {
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public int? YearOfManufacture { get; set; }
        public string? Status { get; set; }


        // Navigation Properties TODO: Uncomment and implement related entities
        // public ICollection<Component> Components { get; set; } = new List<Component>();
        // public ICollection<MaintenanceEvent> MaintenanceEvents { get; set; } = new List<MaintenanceEvent>();
    }
}