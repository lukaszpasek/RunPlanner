using Microsoft.AspNetCore.Mvc;
using RunPlanner.Models;
using System.Diagnostics;

namespace RunPlanner.Controllers
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

        public IActionResult Sample()
        {
            return View();
        }

        public IActionResult GoogleMaps()
        {
            return View();
        }

        public IActionResult OpenStreet()
        {
            return View();
        }

        public IActionResult HEREMap()
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
