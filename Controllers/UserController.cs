using ASP_spr321.Data;
using ASP_spr321.Models.Home;

using ASP_spr321.Models.User;
using ASP_spr321.Services.Kdf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Text.Json;

namespace ASP_spr311.Controllers
{
    public class UserController(DataContext dataContext, IKdfService kdfService) : Controller
    {
        private const String signupFormKey = "UserSinnupFormModel";
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService=kdfService;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            UserSinnupViewModel viewModel = new();

            // перевіряємо чи є збережені у сесії дані від форми Register
            if (HttpContext.Session.Keys.Contains(signupFormKey))
            {
                // відновлюємо об'єкт моделі з серіалізованого стану
                viewModel.FormModel =
                    JsonSerializer.Deserialize<UserSinnupFormModel>(
                        HttpContext.Session.GetString(signupFormKey)!
                    );
                // проводимо валідацію переданих даних
                viewModel.ValidationErrors = ValidateSinnupFormModel(viewModel.FormModel);

                if (viewModel.ValidationErrors.Count == 0)
                {
                    Guid userId = Guid.NewGuid();
                    _dataContext.UsersData.Add(new()
                    {
                        Id = userId,
                        Name = viewModel.FormModel!.UserName,
                        Email = viewModel.FormModel!.UserEmail,
                        Phone = viewModel.FormModel.UserPhone,
                        DateBirth = viewModel.FormModel!.DateBirth,
                        ShoeSize = viewModel.FormModel.ShoeSize,
                        ClothingSize = viewModel.FormModel.ClothingSize,
                        FingerSize = viewModel.FormModel.FingerSize,
                    });
                    String salt = Guid.NewGuid().ToString()[..16];
                    _dataContext.UserAccesses.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Login = viewModel.FormModel!.UserLogin,
                        RoleId = "guest",
                        Salt = salt,
                        Dk =_kdfService.DerivedKey (
                            viewModel.FormModel!.UserPassword,
                            salt),



                    });
                    _dataContext.SaveChanges();
                }


                 // видаляємо з сесії вилучені дані
                 HttpContext.Session.Remove(signupFormKey);
            }
            return View(viewModel);
        }

        public IActionResult Signin()
        {
            // 'Basic' HTTP Authentication Scheme  https://datatracker.ietf.org/doc/html/rfc7617#section-2
            // Дані автентифікації приходять у заголовку Authorization
            // за схемою  Authentication: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==
            // де дані - Base64 закодована послідовність "login:password"
            String authHeader = Request.Headers.Authorization.ToString();
            if (String.IsNullOrEmpty(authHeader))
            {
                return Json(new { status = 401, message = "Authorization header required" });
            }
            String scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                return Json(new { status = 401, message = $"Authorization scheme must be {scheme}" });
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
                return Json(new { status = 401, message = $"Not valid Base64 code '{credentials}'" });
            }
            // authData == "login:password"
            String[] parts = authData.Split(':', 2);
            if (parts.Length != 2)
            {
                return Json(new { status = 401, message = $"Not valid credentials format (missing ':'?)" });
            }
            String login = parts[0];
            String password = parts[1];
            var userAccess = _dataContext.UserAccesses.FirstOrDefault(ua => ua.Login == login);
            if (userAccess == null)
            {
                return Json(new { status = 401, message = "Credentials rejected" });
            }
            if (_kdfService.DerivedKey(password, userAccess.Salt) != userAccess.Dk)
            {
                return Json(new { status = 401, message = "Credentials rejected." });
            }
            // Зберігаємо у сесію відомості про автентифікацію
            HttpContext.Session.SetString("userAccessId", userAccess.Id.ToString());

            return Json(new { status = 200, message = "OK" });
        }



        public RedirectToActionResult Register([FromForm] UserSinnupFormModel formModel)
        {
            HttpContext.Session.SetString(            // Збереження у сесії
                signupFormKey,                // під ключем UserSignupFormModel
                JsonSerializer.Serialize(formModel)   // серіалізованого об'єкту formModel
            );
            return RedirectToAction(nameof(Signup));
        }
        private Dictionary<String, String> ValidateSinnupFormModel(UserSinnupFormModel? formModel)
        {
            Dictionary<String, String> errors = [];
            if (formModel == null)
            {
                errors["Model"] = "Дані не передані";
            }
            else
            {
                if (String.IsNullOrEmpty(formModel.UserName))
                {
                    errors[nameof(formModel.UserName)] = "Ім'я необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.UserEmail))
                {
                    errors[nameof(formModel.UserEmail)] = "E-mail необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.UserPhone))
                {
                    errors[nameof(formModel.UserPhone)] = "Телефон необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.UserLogin))
                {
                    errors[nameof(formModel.UserLogin)] = "Логін необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.DateBirth))
                {
                    errors[nameof(formModel.DateBirth)] = "Дата народження необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.ShoeSize))
                {
                    errors[nameof(formModel.ShoeSize)] = "Розмір взуття необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.ClothingSize))
                {
                    errors[nameof(formModel.ClothingSize)] = "Розмір одягу необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.FingerSize))
                {
                    errors[nameof(formModel.FingerSize)] = "Розмір пальця необхідно ввести";
                }
                else
                {
                    _dataContext
                        .UserAccesses
                        .FirstOrDefault(ua => ua.Login == formModel.UserLogin != null);
                }

                if (String.IsNullOrEmpty(formModel.UserPassword))
                {
                    errors[nameof(formModel.UserPassword)] = "Пароль необхідно ввести";
                }
                if (formModel.UserPassword != formModel.UserRepeat)
                {
                    errors[nameof(formModel.UserRepeat)] = "Паролі не однакові";
                }
            }
            return errors;
        }
    }
}