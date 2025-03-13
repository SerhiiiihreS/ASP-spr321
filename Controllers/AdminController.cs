using ASP_spr321.Data;
using ASP_spr321.Data.Entities;
using ASP_spr321.Migrations;
using ASP_spr321.Models.Admin;
using ASP_spr321.Models.User;
using ASP_spr321.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace ASP_spr321.Controllers
{
    public class AdminController(DataContext dataContext, IStorageService storageService) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IStorageService _storageService = storageService;

        public IActionResult Index()
        {
            String? canCreate = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "CanCreate")?.Value;

            if (canCreate != "1")
            {
                Response.StatusCode = StatusCodes.Status403Forbidden;
                return NoContent();
            }

            return View();
        }

        [HttpPost]
        public JsonResult AddCategory(CategoryFormModel formModel)
        {
            Data.Entities.Category category = new()
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Name = formModel.Name,
                Description = formModel.Description,
                Slug = formModel.Slug,
                ImageUrl = _storageService.SaveFile(formModel.Image)
            };
            if (String.IsNullOrEmpty(category.Name) )
            {
                return Json(new { status = 401, message = "Не всі поля заповнені!" , message2 = "*"});
            }
            if (String.IsNullOrEmpty(category.Description) )
            {
                return Json(new { status = 401, message = "Не всі поля заповнені!" });
            }
            if (String.IsNullOrEmpty(category.Slug))
            {
                return Json(new { status = 401, message = "Не всі поля заповнені!" });
            }
            else
            {
                _dataContext.Categories.Add(category);
                try
                {
                    _dataContext.SaveChanges();
                }
                catch
                {
                    // _storageService.DeleteFile(category.ImageUrl)
                }
                return Json(category);

            }
            
        }
    }
}