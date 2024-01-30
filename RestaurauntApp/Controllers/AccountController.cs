using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.DTOS;
using RestaurauntApp.Repositories.Base;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(IAccountRepository repository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.accountRepository = repository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Пользователь уже аутентифицирован,ему нет смысла быть тут
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken] // атрибут для защиты от CSRF
        public async Task<IActionResult> Register([FromForm] UserDTO newUser)
        {
            try
            {
                var newAccount = new IdentityUser
                {
                    UserName = newUser.Name,
                    // Email = newUser.Email,
                };

                var result = await userManager.CreateAsync(newAccount, newUser.Password);
                System.Console.WriteLine(result.ToString());

                if (result.Succeeded)
                {
                    // Успешная регистрация
                    await signInManager.SignInAsync(newAccount, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Ошибки регистрации
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // Вывод ошибок в консоль
                    var errors = ViewData.ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
                    foreach (var modelState in errors)
                    {
                        foreach (var error in modelState.Value.Errors)
                        {
                            System.Console.WriteLine($"{modelState.Key}: {error.ErrorMessage}");
                        }
                    }

                    // view с ошибкой
                    return View("Register");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }
        [Authorize]//если пользователь зарегистрирован только 
        public IActionResult Profile()
        {
            return View();
        }
    }
}
