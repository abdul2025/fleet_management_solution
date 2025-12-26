using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FleetManagement.Application.Aircrafts.DTOs;
using FleetManagement.Application.Aircrafts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Web.Controllers.Aircrafts
{
    [ApiController]
    [Route("api/aircrafts")]
    public class AircraftsApiController : ControllerBase
    {
        private readonly ILogger<AircraftsApiController> _logger;
        private readonly IAircraftService _service;

        public AircraftsApiController(IAircraftService service, ILogger<AircraftsApiController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("upload-json")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UploadJson([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            IEnumerable<AircraftDto> dtos;

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                var json = await reader.ReadToEndAsync();
                dtos = JsonSerializer.Deserialize<IEnumerable<AircraftDto>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AircraftDto>();
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON format.");
            }

            var result = await _service.ImportFromJsonAsync(dtos);

            return Ok(new
            {
                successCount = result.SuccessCount,
                errors = result.Errors
            });
        }
    }

}