using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FleetManagement.Web.Models;
using FleetManagement.Application.Aircrafts.Interfaces;
using FleetManagement.Application.Aircrafts.DTOs;
using FleetManagement.Domain.Enums;

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
        public async Task<IActionResult> Index()
        {
            var aircrafts = await _service.GetAllAsync();
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



        public async Task<IActionResult> AircraftListPartial()
        {
            var aircrafts = await _service.GetAllAsync();
            if (!aircrafts.Any())
            {
                return PartialView("_EmptyAircraftListPartial");
            }
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


        public async Task<IActionResult> AircraftStatsPartial()
        {
            var aircrafts = await _service.GetAllAsync();
            return PartialView("_AircraftStatsPartial", aircrafts);
        }




        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                var allAircrafts = await _service.GetAllAsync();
                return PartialView("_AircraftListPartial", allAircrafts);
            }

            // Implement search in the service
            var result = await _service.SearchAsync(query);

            if (!result.Any())
                return PartialView("_EmptyAircraftListPartial");

            return PartialView("_AircraftListPartial", result);
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