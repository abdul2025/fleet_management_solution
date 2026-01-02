using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.Enums;
namespace FleetManagement.Application.Aircrafts.DTOs
{

    public class AircraftDto
    {
        public int Id { get; set; } 
        public string RegistrationNumber { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public AircraftManufacturers Manufacturer { get; set; }
        public int? YearOfManufacture { get; set; }
        public AircraftStatus Status { get; set; }

        public AircraftSpecificationDto Specification { get; set; } = new AircraftSpecificationDto();
    }
}