using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunPlanner.Models;
using System.Diagnostics;

namespace RunPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GpxDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, GpxDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BingMap()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GoogleMaps()
        {
            return View();
        }
        [HttpGet]
        public IActionResult OpenStreet()
        {
            return View();
        }
        [HttpGet]
        public IActionResult HEREMap()
        {
            return View();
        }
        // HomeController.cs
        [HttpPost]
        public IActionResult AddGpxTrack(GpxTrackViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tutaj dodaj logikę do zapisywania trasy GPX do bazy danych
                // Użyj modelu GpxTrackViewModel do przechwycenia danych z formularza

                // Przykładowa logika:
                var gpxTrack = new GpxTrack
                {
                    // Ustaw właściwości trasy GPX na podstawie danych z formularza
                    Name = model.Name,
                    // ...
                };

                // Dodaj do kontekstu bazy danych i zapisz zmiany
                _dbContext.GpxTracks.Add(gpxTrack);
                _dbContext.SaveChanges();

                // Przekieruj do strony po pomyślnym dodaniu trasy
                return RedirectToAction("Index");
            }

            // Jeśli model nie jest poprawny, wróć do formularza z błędami
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
