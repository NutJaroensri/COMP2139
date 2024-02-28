using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication2.Models;

namespace WebApplication2.Controllers
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

        [Route("/privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/about")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GeneralSearch(string searchType, string searchString)
        {
            if (searchType == "Flight")
            {
                //Redirect to Flights search
                int defaultBookingId = 1;
                return RedirectToAction("Search", "Flight", new { searchString });
            }
            else if (searchType == "Rental")
            {
                //Redirect to RentalCars search
                int defaultBookingId = 1;
                return RedirectToAction("Search", "Rental", new { searchString });
            }

            else if (searchType == "Hotel")
            {
                //Redirect to Hotels search
                int defaultBookingId = 1;
                return RedirectToAction("Search", "Hotel", new { searchString });
            }


            return RedirectToAction("Index", "Home");

        }

        public IActionResult NotFound(int statusCode)
        {
            _logger.LogInformation("Not Found action called with status code: {StatusCode}", statusCode);

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
