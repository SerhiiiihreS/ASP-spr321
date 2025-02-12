using System.Diagnostics;
using ASP_spr321.Models;
using ASP_spr321.Services.OTP;
using ASP_spr321.Services.Timestamp;
using Microsoft.AspNetCore.Mvc;

namespace ASP_spr321.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OTPservice _otpService;
        private readonly ITimestampService _timestampService;

        public HomeController(ILogger<HomeController> logger, ITimestampService timestampService,OTPservice otpService)
        {
            _logger = logger;
            _timestampService= timestampService;
            _otpService = otpService;
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
            ViewData["timestampCode"] = _timestampService.GetHashCode();
            return View();
        }

        public IActionResult OTP()
        {
            ViewData["OTPLen"] = _otpService.OTPLength;
            ViewData["OTP"] = _otpService.OTP;
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
