using System.Diagnostics;
using System.Text.Json;
using ASP_spr321.Models;
using ASP_spr321.Models.Home;
using ASP_spr321.Services.OTP;
using ASP_spr321.Services.Timestamp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

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
        public ViewResult Models()

        {
            HomeModelsViewModel viewModel = new();
            if (HttpContext.Session.Keys.Contains("HomeModelsFormModel"))
            {
                viewModel.FormModel =
                    JsonSerializer.Deserialize<HomeModelsFormModel>(
                        HttpContext.Session.GetString("HomeModelsFormModel")
                    );
                HttpContext.Session.Remove("HomeModelsFormModel");
            }
         
            return View(viewModel);
        }
        public RedirectToActionResult Register(HomeModelsFormModel formModel)
        {
            HttpContext.Session.SetString("HomeModelsFormModel",
                JsonSerializer.Serialize(formModel)
            );
            //HomeModelsViewModel viewModel= new()
            //{
            //    FormModel = formModel
            //};
           
            return RedirectToAction(nameof(Models) );
        }
        public JsonResult Ajax(HomeModelsFormModel formModel)
        {
            return Json(formModel);
        }

        public JsonResult AjaxJson([FromBody]HomeModelsAjaxModel formModel)
        {
            return Json(formModel);
        }

        public ViewResult ProductReviewForm()

        {
            HomeModelsViewModel viewModel = new();
            if (HttpContext.Session.Keys.Contains("HomeModelsFormModel"))
            {
                viewModel.FormModel =
                    JsonSerializer.Deserialize<HomeModelsFormModel>(
                        HttpContext.Session.GetString("HomeModelsFormModel")
                    );
                HttpContext.Session.Remove("HomeModelsFormModel");
            }

            return View(viewModel);
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
