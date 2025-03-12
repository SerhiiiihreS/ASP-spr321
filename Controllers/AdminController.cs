using ASP_spr321.Data;
using ASP_spr321.Data.Entities;
using ASP_spr321.Models.Admin;
using ASP_spr321.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_spr321.Controllers
{
    public class AdminController(DataContext dataContext, IStorageService storageService) : Controller
    {
        private readonly DataContext _dataContext=dataContext;
        private readonly IStorageService _storageService=storageService;
        public IActionResult Index()
        {
            String? canCreate = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "CanCreate")?.Value;
                if(canCreate != "1")
                {
                    Response.StatusCode=StatusCodes.Status403Forbidden;
                    return NoContent();
                }
                
            return View();
        }

        [HttpPost]
        public JsonResult AddCategory(CategoryFormModel formModel)
        {
            Category category = new()
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Name = formModel.Name,
                Description = formModel.Description,
                Slug = formModel.Slug,
                ImageUrl = _storageService.SaveFile(formModel.Image)
            };
                
            _dataContext.Categories.Add(category);
            _dataContext.SaveChanges();
            return Json(category);
        }
    }
}
