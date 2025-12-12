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
        public async Task<IActionResult> Index()
        {
            var aircrafts = await _service.GetAllAsync();
            return View(aircrafts);
        }

        // GET: Aircrafts/GetCreateModal
        public IActionResult GetCreateModal()
        {
            PopulateDropdowns();
            return PartialView("_CreateEditModal", new AircraftDto 
            { 
                Specification = new AircraftSpecificationDto() 
            });
        }

        // GET: Aircrafts/GetEditModal/5
        public async Task<IActionResult> GetEditModal(int id)
        {
            var aircraft = await _service.GetByIdAsync(id);
            
            if (aircraft == null)
            {
                return Json(new { success = false, message = "Aircraft not found." });
            }

            PopulateDropdowns();
            return PartialView("_CreateEditModal", aircraft);
        }

        // POST: Aircrafts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AircraftDto aircraft)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    
                    return Json(new 
                    { 
                        success = false, 
                        message = "Please correct the validation errors.",
                        errors = errors
                    });
                }

                var createdAircraft = await _service.CreateAsync(aircraft);
                var allAircrafts = await _service.GetAllAsync();
                
                return Json(new 
                { 
                    success = true, 
                    message = "Aircraft created successfully!",
                    data = createdAircraft,
                    allData = allAircrafts
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating aircraft");
                return Json(new 
                { 
                    success = false, 
                    message = "An error occurred while creating the aircraft.",
                    error = ex.Message
                });
            }
        }

        // POST: Aircrafts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AircraftDto aircraft)
        {
            try
            {
                if (id != aircraft.Id)
                {
                    return Json(new 
                    { 
                        success = false, 
                        message = "Invalid aircraft ID."
                    });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    
                    return Json(new 
                    { 
                        success = false, 
                        message = "Please correct the validation errors.",
                        errors = errors
                    });
                }

                var result = await _service.UpdateAsync(id, aircraft);
                
                if (result == null)
                {
                    return Json(new 
                    { 
                        success = false, 
                        message = "Aircraft not found."
                    });
                }

                var allAircrafts = await _service.GetAllAsync();
                
                return Json(new 
                { 
                    success = true, 
                    message = "Aircraft updated successfully!",
                    data = result,
                    allData = allAircrafts
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating aircraft with ID {Id}", id);
                return Json(new 
                { 
                    success = false, 
                    message = "An error occurred while updating the aircraft.",
                    error = ex.Message
                });
            }
        }

        // POST: Aircrafts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                
                if (!result)
                {
                    return Json(new 
                    { 
                        success = false, 
                        message = "Aircraft not found or could not be deleted."
                    });
                }

                var allAircrafts = await _service.GetAllAsync();
                
                return Json(new 
                { 
                    success = true, 
                    message = "Aircraft deleted successfully!",
                    deletedId = id,
                    allData = allAircrafts
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting aircraft with ID {Id}", id);
                return Json(new 
                { 
                    success = false, 
                    message = "An error occurred while deleting the aircraft.",
                    error = ex.Message
                });
            }
        }

        // Helper method to populate dropdowns
        private void PopulateDropdowns()
        {
            ViewBag.Manufacturers = Enum.GetValues(typeof(AircraftManufacturers))
                .Cast<AircraftManufacturers>()
                .Select(m => new SelectListItem
                {
                    Value = ((int)m).ToString(),
                    Text = m.ToString()
                })
                .ToList();

            ViewBag.Statuses = Enum.GetValues(typeof(AircraftStatus))
                .Cast<AircraftStatus>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                })
                .ToList();

            ViewBag.WeightUnits = Enum.GetValues(typeof(WeightUnit))
                .Cast<WeightUnit>()
                .Select(w => new SelectListItem
                {
                    Value = ((int)w).ToString(),
                    Text = w.ToString()
                })
                .ToList();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}