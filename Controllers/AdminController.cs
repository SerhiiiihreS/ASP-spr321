using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_spr321.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            String? canCreate = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "CanCreate")?.Value;
                if(canCreate != "1")
                {
                    return Forbid();
                }
                
            return View();
        }
    }
}
