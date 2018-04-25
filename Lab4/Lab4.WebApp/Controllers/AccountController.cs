using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Lab4.BLL.Models;
using Lab4.BLL.Services;
using Lab4.BLL;
using Lab4.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Lab4.WebApp
{
    public class AccountController : Controller
    {
        private UserService userService;
        public AccountController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid) {
                User user = userService.SignIn(model.Login, model.Password);
                if (user != null) {
                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login/password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid) {
                User user = userService.SignIn(model.Email, model.Password);
                if (user == null) {
                    user = userService.SignUp(new User() {
                        Login = model.Email,
                        Email = model.Email,
                        Password = model.Password,
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    });

                    await Authenticate(model.Email);

                    return RedirectToAction("Index", "Home");
                }
                else {
                    ModelState.AddModelError("", "Account already exists. Sign in");
                }
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}