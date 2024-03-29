using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurauntApp.DTOS;
#pragma warning disable CS8602
namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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
        public async Task<IActionResult> Register([FromForm] UserDTO newUser)
        {
            try
            {
                var existingUser = await userManager.FindByNameAsync(newUser.Name);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "This username is already taken.");
                    return View("Register");
                }

                var newAccount = new IdentityUser
                {
                    UserName = newUser.Name,
                };

                var result = await userManager.CreateAsync(newAccount, newUser.Password);
                System.Console.WriteLine(result.ToString());

                if (result.Succeeded)
                {
                    // Успешная регистрация
                    var isAdmin = newUser.Name.Contains("Admin", StringComparison.CurrentCultureIgnoreCase);
                    if (isAdmin)
                    {
                        var role = new IdentityRole { Name = "Admin" };
                        await roleManager.CreateAsync(role);

                        await userManager.AddToRoleAsync(newAccount, role.Name);

                    }

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return base.RedirectToAction(actionName: "GetAll", controllerName: "Menu");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(user.Name, user.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync(string OldPassword, string newPassword)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            // Проверяем, является ли введенный старый пароль текущим паролем пользователя
            var signInResult = await signInManager.CheckPasswordSignInAsync(user, OldPassword, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Incorrect old password.");
                return View("ChangePassword");
            }
            // Старый пароль верен, продолжаем смену пароля
            var changePasswordResult = await userManager.ChangePasswordAsync(user, OldPassword, newPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("ChangePassword");
            }

            return RedirectToAction("Profile");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var removeAccountResult = await userManager.DeleteAsync(user);
            if (!removeAccountResult.Succeeded)
            {
                var errors = removeAccountResult.Errors.Select(e => e.Description);
                return BadRequest(new { errors });
            }

            return RedirectToActionPermanent("Index", "Home");
        }
    }
}
