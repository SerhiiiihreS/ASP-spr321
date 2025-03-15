using ASP_spr321.Data;
using ASP_spr321.Data.Entities;
using ASP_spr321.Migrations;
using ASP_spr321.Models.Admin;
using ASP_spr321.Models.User;
using ASP_spr321.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
            AdminIndexViewModel viewModel = new()
            {
                Categories = _dataContext.Categories.ToList(),
            };

            return View(viewModel);
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
            if (String.IsNullOrEmpty(category.Name) && String.IsNullOrEmpty(category.Description) && String.IsNullOrEmpty(category.Slug))
            {
                return Json(new { status = 400, message = "Поля пусті!" });
            }
            else
            {
                if (String.IsNullOrEmpty(category.Name) || String.IsNullOrEmpty(category.Description) || String.IsNullOrEmpty(category.Slug))
                {
                    return Json(new { status = 401, message = "Не всі поля заповнені!" });
                }
                if (String.IsNullOrEmpty(category.Name) && String.IsNullOrEmpty(category.Description) || String.IsNullOrEmpty(category.Slug))
                {
                    return Json(new { status = 401, message = "Не всі поля заповнені!" });
                }
                if (String.IsNullOrEmpty(category.Name) || String.IsNullOrEmpty(category.Description) && String.IsNullOrEmpty(category.Slug))
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
                        //_storageService.DeleteFile(category.ImageUrl)
                    }
                    return Json(category);

                }

            }


        }

        [HttpPost]
        public JsonResult AddProduct(ProductFormModel formModel)
        {
            double price;
            try
            {
                price = double.Parse(formModel.Price, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                price = double.Parse(formModel.Price.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
            }
            Product product = new()
            {
                Id = Guid.NewGuid(),
                CategoryId = formModel.CategoryId,
                Name = formModel.Name,
                Description = formModel.Description,
                Slug = formModel.Slug,
                Price =price,

                Stock = formModel.Stock,
                ImagesCsv = String.Join(',',
                         formModel
                         .Images
                         .Select(img => _storageService.SaveFile(img))
                         ),
            };


            _dataContext.Products.Add(product);
            try
            {
                _dataContext.SaveChanges();
            }
            catch
            {
                   
                //_storageService.DeleteFile(category.ImageUrl)
            }
            return Json(product);
        }
    }
}