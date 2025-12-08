using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FleetManagement.Web.Models;


namespace FleetManagement.Web.Controllers.Aircrafts
{
    public class AircraftsController : Controller
    {
        private readonly ILogger<AircraftsController> _logger;

        public AircraftsController(ILogger<AircraftsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}