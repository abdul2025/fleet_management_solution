using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FleetManagement.Web.Models;
using FleetManagement.Application.Aircrafts.Interfaces;
using FleetManagement.Application.Aircrafts.DTOs;
using FleetManagement.Shared.Enums;
using FleetManagement.Shared.Enums.AircraftEnums;

namespace FleetManagement.Web.Controllers.Aircrafts
{
    public class AircraftsController : Controller
    {
        private readonly ILogger<AircraftsController> _logger;
        private readonly IAircraftService _service;

        public AircraftsController(IAircraftService service, ILogger<AircraftsController> logger)
        {
            _logger = logger;
            _service = service;
        }

        // GET: Aircraft
        public async Task<IActionResult> Index([FromQuery] AircraftFilterDto? filter)
        {
            IEnumerable<AircraftDto> aircrafts;

            if (filter != null && (filter.Status.HasValue || !string.IsNullOrWhiteSpace(filter.Registration)
                || !string.IsNullOrWhiteSpace(filter.BasedStation) || !string.IsNullOrWhiteSpace(filter.Model)
                || filter.YearFrom.HasValue || filter.YearTo.HasValue))
            {
                aircrafts = await _service.GetFilteredAsync(filter);
            }
            else
            {
                aircrafts = await _service.GetAllAsync();
            }

            return View(aircrafts);
        }



        // GET: Aircraft/Create for the modal
        public async Task<IActionResult> CreateModal()
        {
            var dto = new AircraftDto(); // For creation
            PopulateDropdowns(dto);
            return PartialView("_CreateEditModal", dto);
        }

        public async Task<IActionResult> EditModal(int id)
        {
            var aircraft = await _service.GetByIdAsync(id); // FIXED

            if (aircraft == null)
                return NotFound();

            PopulateDropdowns(aircraft);
            return PartialView("_CreateEditModal", aircraft);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AircraftDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new { success = false, errors });
                }

                await _service.CreateAsync(dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating aircraft");
                return Json(new { success = false, errors = new { General = new[] { "An unexpected error occurred. Please try again." } } });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AircraftDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PopulateDropdowns(dto);
                    var errors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new { success = false, errors });
                }

                await _service.UpdateAsync(id, dto);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating aircraft with ID {id}");
                return Json(new { success = false, errors = new { General = new[] { "An unexpected error occurred. Please try again." } } });
            }
        }



        public async Task<IActionResult> AircraftListPartial([FromQuery] AircraftFilterDto? filter)
        {
            IEnumerable<AircraftDto> aircrafts;

            if (filter != null)
                aircrafts = await _service.GetFilteredAsync(filter);
            else
                aircrafts = await _service.GetAllAsync();

            if (!aircrafts.Any())
                return PartialView("_EmptyAircraftListPartial");

            return PartialView("_AircraftListPartial", aircrafts);
        }


        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        // TODO: this is not working correctly
        [HttpGet]
        public async Task<IActionResult> AircraftStatsPartial()
        {
            var aircrafts = _service.GetAllAsync();
            return PartialView("_AircraftStatsPartial", aircrafts);
        }








        private void PopulateDropdowns(AircraftDto dto)
{
            ViewBag.Manufacturers = Enum.GetValues(typeof(AircraftManufacturers))
                .Cast<AircraftManufacturers>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString(), Selected = x == dto.Manufacturer });

            ViewBag.Statuses = Enum.GetValues(typeof(AircraftStatus))
                .Cast<AircraftStatus>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString(), Selected = x == dto.Status });

            ViewBag.WeightUnits = Enum.GetValues(typeof(WeightUnit))
                .Cast<WeightUnit>()
                .Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString(), Selected = x == dto.Specification.WeightUnit });
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}