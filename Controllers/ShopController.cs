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
            Cart? cart = _dataContext
                    .Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefault(c => c.UserAccessId.ToString() == uaId);

            ShopCartViewModel viewModel = new()
            {
                Products = _dataContext.Products.ToList(),
                Cart =cart==null? null:
                    cart with {
                        CartItems = cart.CartItems.Select(ci => ci with
                        {
                            Product = ci.Product with {
                                ImagesCsv= String.IsNullOrEmpty(ci.Product.ImagesCsv)
                                   ? "/Shop/Image/no-image.jpg"
                                   : ci.Product.ImagesCsv = String.Join(',',
                                        ci.Product.ImagesCsv
                                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(img => "/Shop/Image/" + img))
                             }
                        })
                        .ToList()
                    } 
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



        [HttpPut]
        public JsonResult ModifyCartItem([FromQuery] String cartId, [FromQuery] int delta)
        {
            if (delta == 0)
            {
                return Json(new { Status = 400, Message = "No action needed for 0 delta" });
            }
            Guid cartGuid;
            try { cartGuid = Guid.Parse(cartId); }
            catch { return Json(new { Status = 400, Message = "Invalid cartId format: UUID expected" }); }

            CartItem? cartItem = _dataContext.CartItems
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .FirstOrDefault(ci => ci.Id == cartGuid);
            if (cartItem == null)
            {
                return Json(new { Status = 404, Message = "No item with requested cartId" });
            }
            int newQuantity = cartItem.Quantity + delta;
            
            if (newQuantity < 0)
            {
                return Json(new { Status = 400, Message = "Invalid delta: negative total quantity" });
            }
            if (newQuantity > cartItem.Product.Stock)
            {
                return Json(new { Status = 422, Message = "Delta too large: stock limit exceeded" });
            }
            if (newQuantity == 0)
            {
                cartItem.Cart.Price -= cartItem.Price;
                _dataContext.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Cart.Price += delta * cartItem.Product.Price;  // + Actions
                cartItem.Price += delta * cartItem.Product.Price;  // + Actions
                cartItem.Quantity = newQuantity;
                
            }
            _dataContext.SaveChanges();
            return Json(new { Status = 200, Message = "Modifed" });
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
