using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagement.Application.Aircrafts.DTOs
{
    public class ImportResultDto
    {
        public int SuccessCount { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}