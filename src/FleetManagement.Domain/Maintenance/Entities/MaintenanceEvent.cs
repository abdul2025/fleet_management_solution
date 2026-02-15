using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.Aircrafts.Entities;
using FleetManagement.Domain.CommonEntities;
using FleetManagement.Domain.Enums.Maintenance;
using FleetManagement.Domain.Components.Entities;


namespace FleetManagement.Domain.Maintenance.Entities
{
    public class MaintenanceEvent : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Aircraft))]
        public int AircraftId { get; set; }

        // Optional: maintenance can be specific to a component
        [ForeignKey(nameof(AircraftComponent))]
        public int? ComponentId { get; set; }

        [Required, StringLength(100)]
        public required string EventType { get; set; } // e.g., "A Check", "B Check", "Unscheduled"

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public MaintenanceStatus Status { get; set; }

        [Required]
        public DateTime PerformedDate { get; set; }

        public DateTime? NextDueDate { get; set; }

        // Navigation properties
        public Aircraft Aircraft { get; set; } = null!;
        public AircraftComponent? Components { get; set; }
    }
}