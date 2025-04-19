using ASP_spr321.Data.Entities;
using ASP_spr321.Migrations;
using ASP_spr321.Services.Kdf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ASP_spr321.Data
{
    public class DataAccessor(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IKdfService kdfService)
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IKdfService _kdfService = kdfService;
        private String ImagePath => $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Shop/Image/";
        public List<Entities.Category> AllCategories()
        {
            var categories = _dataContext
                .Categories
                .Where(c => c.DeletedAt == null)
                .AsNoTracking()
                .ToList();
            foreach (var category in categories)
            {
                category.ImageUrl = ImagePath + category.ImageUrl;
            }
            return categories;
        }

        public Entities.Category? GetCategory(String slug)
        {
            var category = _dataContext
                .Categories
                .Include(c => c.Products)
                .AsNoTracking()
                .FirstOrDefault(c => c.Slug == slug)
                ;

            if (category != null)
            {
                category.ImageUrl = ImagePath + category.ImageUrl;
                foreach (var product in category!.Products)
                {
                    product.ImagesCsv = String.Join(',',
                        product.ImagesCsv.Split(',').Select(i => ImagePath + i)
                    );
                }
            }
            return category;
        }
        public Entities.Product? GetProduct(String id)
        {
            var product = _dataContext
                 .Products
                 .Include(p => p.Category)
                 .FirstOrDefault(p => p.Slug == id || p.Id.ToString() == id);

            if (product?.Category != null)
            {
                product.ImagesCsv = String.Join(',',
                    product.ImagesCsv.Split(',').Select(i => ImagePath + i)
                );
            }

            return product;
        }
        public Entities.Product? GetProduct2(String id)
        {
            if (!Guid.TryParse(id, out Guid productId))
            {
                return null; // Return null if the provided id is not a valid Guid  
            }

            var product = _dataContext
                .Products
                .FirstOrDefault(p => p.Id == productId);

            if (product?.Category != null)
            {
                product.ImagesCsv = String.Join(',',
                    product.ImagesCsv.Split(',').Select(i => ImagePath + i)
                );
            }

            return product;
        }
        public List<Entities.Product> PopularProducts()
        {
            var products = _dataContext
                .Products
                .Where(c => c.Stock  < 4)
                .AsNoTracking()
                .ToList();
            foreach (var product in products)
            {
                product.ImagesCsv = String.Join(',',
                        product.ImagesCsv.Split(',').Select(i => ImagePath + i)
                    );
            }
            return products;
        }
        public List<Entities.Product> AllProducts()
        {
            var products = _dataContext
                .Products
                .Where(c => c.DeletedAt == null)
                .AsNoTracking()
                .ToList();
            foreach (var product in products)
            {
                product.ImagesCsv = String.Join(',',
                        product.ImagesCsv.Split(',').Select(i => ImagePath + i)
                    );
            }
            return products;
        }


        public AccessToken Authenticate(HttpRequest Request)
        {
            // 'Basic' HTTP Authentication Scheme  https://datatracker.ietf.org/doc/html/rfc7617#section-2
            // Дані автентифікації приходять у заголовку Authorization
            // за схемою  Authentication: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==
            // де дані - Base64 закодована послідовність "login:password"
            String authHeader = Request.Headers.Authorization.ToString();
            if (String.IsNullOrEmpty(authHeader))
            {
                throw new Win32Exception(401, "Authorization header required");
            }
            String scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                throw new Win32Exception(401, $"Authorization scheme must be {scheme}");
            }
            String credentials = authHeader[scheme.Length..];
            String authData;
            try
            {
                authData = System.Text.Encoding.UTF8.GetString(
                    Base64UrlTextEncoder.Decode(credentials)
                );
            }
            catch
            {
                throw new Win32Exception(401, $"Not valid Base64 code '{credentials}'");
            }
            // authData == "login:password"
            String[] parts = authData.Split(':', 2);
            if (parts.Length != 2)
            {
                throw new Win32Exception(401, $"Not valid credentials format (missing ':'?)");
            }
            String login = parts[0];
            String password = parts[1];
            var userAccess = _dataContext.UserAccesses.FirstOrDefault(ua => ua.Login == login);
            if (userAccess == null)
            {
                throw new Win32Exception(401, "Введіть логін!");
            }
            if (_kdfService.DerivedKey(password, userAccess.Salt) != userAccess.Dk && _kdfService.DerivedKey(password, userAccess.Salt) != null)
            {
                throw new Win32Exception(401, "Введіть пароль!");
            }

            AccessToken accessToken = new()
            {
                Jti = Guid.NewGuid(),
                Sub = userAccess.Id,
                Aud = userAccess.UserId,
                Iat = DateTime.Now,
                Nbf = null,
                Exp = DateTime.Now.AddMinutes(10),
                Iss = "asp-spr311"
            };
            _dataContext.AccessTokens.Add(accessToken);
            _dataContext.SaveChanges();
            return accessToken;

        }

    }
}
