﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels.Identity;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager, ILogger<AccountController> Logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = Logger;
        }

        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid)
                return View(Model);

            _Logger.LogInformation("Регистрация нового пользователя {0}", Model.UserName);

            var user = new User
            {
                UserName = Model.UserName
            };

            var registration_result = await _UserManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} успешно зарегистрирован", Model.UserName);
                await _SignInManager.SignInAsync(user, false);
                _Logger.LogInformation("Пользователь {0} вошёл в систему", Model.UserName);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            _Logger.LogWarning("Ошибка при регистрации нового пользователя {0}:{1}",
                Model.UserName, string.Join(", ", registration_result.Errors.Select(e => e.Description)));

            return View(Model);
        }

        public IActionResult Login(string ReturnUrl = null) => View(new LoginUserViewModel { ReturnUrl = ReturnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel Model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RemeberMe,
                true); // Блокировать, если ошибок доступа больше, чем указано в конфигурации

            if (login_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} вошёл в систему", Model.UserName);

                if (Url.IsLocalUrl(Model.ReturnUrl))
                    return Redirect(Model.ReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверное имя пользователя или пароль");

            _Logger.LogWarning("Ошибка при входе пользователя {0} в систему", Model.UserName);
            return View(Model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user_name = User.Identity.Name;
                await _SignInManager.SignOutAsync();
                _Logger.LogInformation("Пользователь {0} вышел в систему", user_name);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index(string ReturnUrl = null)
        {
            return View(new AccountMainViewModel 
            {
                Login = new LoginUserViewModel { ReturnUrl = ReturnUrl },
                Register = new RegisterUserViewModel()

            });
        }

    }
}