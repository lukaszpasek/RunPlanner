using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunPlanner.Models;
using System.Diagnostics;
using System.Xml;
using GeoJSON.Net;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using NuGet.Protocol;

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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GpxTrackViewModel model)
        {
            string returnUrl = Request.Form["returnUrl"];

            try
            {
                if (ModelState.IsValid)
                {
                    var gpxTrack = new GpxTrack
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CreatedAt = model.CreatedAt.UtcDateTime,
                        GpxData = model.GpxData
                    };

                    _dbContext.GpxTracks.Add(gpxTrack);
                    _dbContext.SaveChanges();

                    TempData["SuccessMessage"] = "Form submitted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Form submission failed";
                }
            }
            catch (Exception ex)
            {
                return Content("Wystąpił wyjątek do obsłużenia");
            }

            return Redirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> GetGpxTracks()
        {
            var gpxTracks = await _dbContext.GpxTracks.ToListAsync();

            var gpxTracksDto = gpxTracks.Select(track => new
            {
                Id = track.Id,
                Name = track.Name
            });

            return Json(gpxTracksDto);
        }

        static string ConvertGpxToGeoJson(string gpxData)
        {
            var featureCollection = new FeatureCollection();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(gpxData);

            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("gpx", "http://www.topografix.com/GPX/1/1");

            var trackNodes = xmlDoc.SelectNodes("//gpx:trk", nsmgr);

            foreach (XmlNode trackNode in trackNodes)
            {
                var trackPoints = new List<IPosition>();

                var nameNode = trackNode.SelectSingleNode("gpx:name", nsmgr);
                var trackName = nameNode != null ? nameNode.InnerText : "Track";

                var segmentNodes = trackNode.SelectNodes(".//gpx:trkseg/gpx:trkpt", nsmgr);

                if (segmentNodes != null && segmentNodes.Count >= 2)
                {
                    foreach (XmlNode pointNode in segmentNodes)
                    {
                        if (double.TryParse(pointNode.Attributes["lat"]?.Value, out var lat) &&
                            double.TryParse(pointNode.Attributes["lon"]?.Value, out var lon))
                        {
                            trackPoints.Add(new Position(lat, lon));
                        }
                        else
                        {
                            Console.WriteLine("Błąd podczas konwersji współrzędnych.");
                        }
                    }
                    if (trackPoints.Count >= 2)
                    {
                        var coordinates = new LineString(trackPoints);
                        var featureProperties = new Dictionary<string, object> { { "name", trackName } };
                        var feature = new Feature(coordinates, featureProperties);
                        featureCollection.Features.Add(feature);
                    }
                }
            }

            return featureCollection.ToJson();
        }
        [HttpGet]
        public async Task<IActionResult> ConvertTrackGPXToGeoJSON(int trackId)
        {
            var gpxTrack = await _dbContext.GpxTracks.FindAsync(trackId);

            if (gpxTrack == null)
            {
                return NotFound(); 
            }

            var geoJSONData = ConvertGpxToGeoJson(gpxTrack.GpxData);
            ViewData["GeoJSONData"] = Json(new { GeoJSONData = geoJSONData });
            return Json(new { GeoJSONData = geoJSONData });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
