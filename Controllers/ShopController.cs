using ASP_spr321.Data;
using ASP_spr321.Data.Entities;
using ASP_spr321.Models;
using ASP_spr321.Models.Shop;
using ASP_spr321.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ASP_spr321.Controllers
{

    public class ShopController(DataContext dataContext, IStorageService storageService) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IStorageService _storageService = storageService;

        public IActionResult Index()
        {
            ShopIndexViewModel viewModel = new()
            {
                Categories = _dataContext.Categories.ToList()
            };

            return View(viewModel);
        }
        public IActionResult Category([FromRoute] String id)
        {
            ShopCategoryViewModel viewModel = new()
            {
                Categories = _dataContext.Categories.ToList(),
                Category = _dataContext
                .Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Slug == id)
            };

            return View(viewModel);
        }

        public ViewResult Product([FromRoute] String id)
        {
            String controllerName = ControllerContext.ActionDescriptor.ControllerName;
            ShopProductViewModel viewModel = new()
            {
                Product = _dataContext
                .Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Slug == id || p.Id.ToString() == id),

                BreadCrumbs = new() {
                    new BreadCrumb() { Title = "Домашня", Url = "/" },
                    new BreadCrumb() { Title = "Крамниця", Url = $"/{controllerName}" },
                }
            };
            if (viewModel.Product != null)
            {
                viewModel.BreadCrumbs.Add(
                    new BreadCrumb()
                    {
                        Title = viewModel.Product.Category.Name,
                        Url = $"/{controllerName}/{nameof(Category)}/{viewModel.Product.Category.Slug}"
                    });
                viewModel.BreadCrumbs.Add(
                    new BreadCrumb()
                    {
                        Title = viewModel.Product.Name,
                        Url = $"/{controllerName}/{nameof(Product)}/{viewModel.Product.Slug ?? viewModel.Product.Id.ToString()}"
                    });
            }
            return View(viewModel);
        }

        public ViewResult Cart()
        {
            String? uaId = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

            ShopCartViewModel viewModel = new()
            {
                Cart = _dataContext
                    .Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefault(c => c.UserAccessId.ToString() == uaId)
            };

            return View(viewModel);
        }



        [HttpPost]
        public JsonResult AddToCart([FromForm] String productId, [FromForm] String uaId)
        {
            if (productId.Length == 36 && productId[8] == '-' && productId[13] =='-' && productId[18] == '-' && productId[23] == '-' 
                && uaId.Length == 36 && uaId[8] == '-' && uaId[13] == '-' && uaId[18] == '-' && uaId[23] == '-')
            {
                Product? product = _dataContext.Products
               .FirstOrDefault(p => p.Id.ToString() == productId);
                if (product == null)
                {
                    return Json(new { Status = 404 });
                }
                /* Перевіряємо чи є в користувача незакритий кошик.
                   якщо є, то доповнюємо його, якщо немає - створюємо новий. */
                Cart? cart = _dataContext.Carts
                    .FirstOrDefault(c => c.UserAccessId.ToString() == uaId);
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        Id = Guid.NewGuid(),
                        UserAccessId = Guid.Parse(uaId),
                        OpenAt = DateTime.Now,
                    };
                    _dataContext.Carts.Add(cart);
                }
                // Те ж саме для CartItem
                CartItem? cartItem = _dataContext.CartItems
                    .FirstOrDefault(ci => ci.CartId == cart.Id &&
                        ci.ProductId.ToString() == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity += 1;
                    cartItem.Price += product.Price;   // перерахунок по акціях
                }
                else
                {
                    cartItem = new CartItem()
                    {
                        Id = Guid.NewGuid(),
                        CartId = cart.Id,
                        ProductId = product.Id,
                        Quantity = 1,
                        Price = product.Price,   // перерахунок по акціях
                    };
                    _dataContext.CartItems.Add(cartItem);
                }
                cart.Price += product.Price;   // перерахунок по акціях

                _dataContext.SaveChanges();
                return Json(new { Status = 200, message = "Формати відповідають UUID!" });

            } 
            else{
                return Json(new { Status = 404, message = "Формати не відповідають UUID!" });
            }

            
            
        }

        public FileResult Image([FromRoute] String id)
        {
            return File(
                System.IO.File.ReadAllBytes(
                    _storageService.GetRealPath(id)),
                "image/jpeg", false
            );
        }
    }
}
