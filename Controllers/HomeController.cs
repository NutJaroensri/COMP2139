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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

		[HttpGet]
		public IActionResult GeneralSearch(string searchType, string searchString)
		{
			if (searchType == "Projects")
			{
				// Redirect to Project search
				return RedirectToAction("Search", "Projects", new { searchString });
			}
			else if (searchType == "Tasks")
			{
				//Redirect to Tasks search
				int defaultProjectId = 1;
				return RedirectToAction("Search", "Tasks", new { projectId = defaultProjectId, searchString });
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
