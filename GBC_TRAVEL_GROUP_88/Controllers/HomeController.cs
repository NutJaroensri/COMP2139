using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GBC_TRAVEL_GROUP_88.Models;
using Microsoft.AspNetCore.Authorization;

namespace GBC_TRAVEL_GROUP_88.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

		[HttpGet]
        [Authorize]
		public IActionResult GeneralSearch(string searchType, string searchString)
		{
			if (searchType == "Flights")
			{
				// Redirect to Project search
				return RedirectToAction("Search", "Flights", new { area = "BookingManagement", searchString });
			}
			else if (searchType == "FlightBookings")
			{

                return RedirectToAction("Search", "FlightBookings", new { area = "BookingManagement", searchString });
            }
            else if (searchType == "Hotels")
            {

                return RedirectToAction("Search", "Hotels", new { area = "BookingManagement", searchString });
            }

            return RedirectToAction("Index", "Home");
		}

		public IActionResult NotFound(int statusCode)
		{
			if (statusCode == 404)
			{
				return View("NotFound");
			}
			return View("Error");
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}
