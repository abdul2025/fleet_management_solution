using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Domain.CommonEntities;
using FleetManagement.Domain.Enums.ComponentEnums;
using FleetManagement.Domain.Maintenance.Entities;

namespace FleetManagement.Domain.Components.Entities
{
    public class AircraftComponent : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public required string Tag { get; set; }

        [Required, StringLength(100)]
        public required string PartNumber { get; set; }

        [Required, StringLength(100)]
        public required string SerialNumber { get; set; }

        [Range(0, 100000)]
        public int? LifeLimitHours { get; set; }

        [Range(0, 100000)]
        public int? LifeLimitCycles { get; set; }

        [Required]
        public int InstalledOnAircraftId { get; set; }

        [Required]
        public DateTime InstalledDate { get; set; }

        [Required]
        public ComponentsStatus Status { get; set; }

        // Navigation property to Aircraft
        public Aircraft InstalledOnAircraft { get; set; } = null!;

        public ICollection<MaintenanceEvent> MaintenanceEvents { get; set; } = new List<MaintenanceEvent>();

    }
}