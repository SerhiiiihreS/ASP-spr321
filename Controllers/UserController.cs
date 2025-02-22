using ASP_spr321.Data;
using ASP_spr321.Models.Home;

using ASP_spr321.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Text.Json;

namespace ASP_spr311.Controllers
{
    public class UserController(DataContext dataContext) : Controller
    {
        private const String signupFormKey = "UserSinnupFormModel";
        private readonly DataContext _dataContext = dataContext;
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
                    });
                    String salt = "salt";
                    _dataContext.UserAccesses.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        Login = viewModel.FormModel!.UserLogin,
                        RoleId = "guest",
                        Salt = salt,
                        Dk = salt + viewModel.FormModel!.UserPassword,



                    });
                    _dataContext.SaveChanges();
                }


                 // видаляємо з сесії вилучені дані
                 HttpContext.Session.Remove(signupFormKey);
            }
            return View(viewModel);
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