using Clinic_system.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Clinic_system.Services;
using Clinic_system.Models;
using Microsoft.AspNetCore.Identity;
using Clinic_system.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Clinic_system.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id))
            {
                var account = await _userService.GetByIdAsync(id);
                ViewBag.FullName = account.FullName;
            }
            return RedirectToAction("Index","Dashboard");
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Account");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await _userService.GetUserByEmailAsync(model.Email);
                if (account != null && PasswordHelper.VerifyPassword(account.Password, model.Password))
                {
                    ClaimsIdentity cp = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    cp.AddClaims([
                        new (ClaimTypes.Name, account.FullName),
                        new (ClaimTypes.Role, account.Role.RoleName ?? "Patient"),
                        new (ClaimTypes.Email, account.Email),
                        new (ClaimTypes.NameIdentifier,account.UserId.ToString()),
                        ]);

                    var user = new ClaimsPrincipal(cp);
                    await HttpContext.SignInAsync(user);
                    return RedirectToAction("index", "home");
                }
                ModelState.AddModelError("", "Invalid Email or Password");
                return View();
            }
            ModelState.AddModelError("", "You must enter email and password");
            return View();
        }

        /*
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Account");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var password = PasswordHelper.HashPassword(model.Password);
                User user = new()
                {
                    RoleId = 3,
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = password,
                    PhoneNumber = model.PhoneNumber != null ? model.PhoneNumber : null,
                    BirthDate = model.BirthDate != null ? model.BirthDate : null,
                    Gender = model.Gender != null ? (model.Gender == "m" ? "Male" : (model.Gender == "f" ? "Female" : null)) : null
                };
                await _userService.AddAsync(user);
            }
            else
            {
                ModelState.AddModelError("", "An Error has occured please try again");
                return RedirectToAction("Register", "Account");
            }
            return RedirectToAction("Login","Account");
        }
        */
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
