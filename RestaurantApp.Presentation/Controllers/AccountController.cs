using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestaurauntApp.Core.Repositories;
using RestaurauntApp.Core.DTOS;

namespace RestaurauntApp.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                // User is already authenticated, redirect to the home page
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserDTO newUser)
        {
            try
            {
                var claims = new Claim[] {
                    new Claim(ClaimTypes.Name, newUser.Name),
                    new Claim("creation_date_utc", DateTime.UtcNow.ToString())
                };

                var isAdmin = newUser.Name.Contains("Admin", System.StringComparison.CurrentCultureIgnoreCase);
                if (isAdmin)
                {
                    claims = claims.Concat([new Claim(ClaimTypes.Role, "Admin")]).ToArray();

                }
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                Console.WriteLine(claimsIdentity.Claims.Count());
                await HttpContext.SignInAsync(
                    scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                    principal: new ClaimsPrincipal(claimsIdentity)
                );
                await accountRepository.CreateAccountAsync(newUser);
                System.Console.WriteLine(isAdmin);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the request.");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> LogOut()
        {
            await base.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return base.RedirectToAction(actionName: "GetAll", controllerName: "Menu");
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                // User is already authenticated, redirect to the home page
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO user)
        {
            if (ModelState.IsValid)
            {
                var isExists = await accountRepository.CheckLogin(user.Name, user.Password);

                if (isExists)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("creation_date_utc", DateTime.UtcNow.ToString())
            };

                    var isAdmin = user.Name.Contains("Admin", StringComparison.CurrentCultureIgnoreCase);
                    if (isAdmin)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin")); 
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                        principal: new ClaimsPrincipal(claimsIdentity)
                    );

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

            // the model is invalid or authentication failed, return the view
            return View();
        }
    }
    
}

