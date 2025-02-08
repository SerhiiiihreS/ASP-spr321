using System.Diagnostics;
using ASP_spr321.Models;
using ASP_spr321.Services.Timestamp;
using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ITimestampService _timestampService;

        public HomeController(ILogger<HomeController> logger, ITimestampService timestampService)
        {
            _logger = logger;
            _timestampService= timestampService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Intro()
        {
            return View();
        }
        public IActionResult Razor()
        {
            return View();
        }
        public IActionResult IoC()
        {
            ViewData["timestamp"] = _timestampService.Timestamp;
            ViewData["timestampCode"] = _timestampService.Timestamp.GetHashCode();
            return View();
        }
        public IActionResult Privacy()
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
