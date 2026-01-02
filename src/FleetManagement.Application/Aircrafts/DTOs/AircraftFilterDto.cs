using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManagement.Domain.Enums;



namespace FleetManagement.Application.Aircrafts.DTOs
{
    public class AircraftFilterDto
    {
        public AircraftStatus? Status { get; set; }
        public string? Registration { get; set; }
        public string? BasedStation { get; set; }
        public string? Model { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
    }
}