using Clinic_system.Helpers;
using Clinic_system.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Doctors()
        {
            var doctors = await _userService.GetDoctorsAsync();
            return View(doctors);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Receptionists()
        {
            var receptionists = await _userService.GetReceptionistsAsync();
            return View(receptionists);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewUser()
        {
            var roles = await _roleService.GetAllAsync();
            ViewBag.Roles = new SelectList(roles,"RoleId","RoleName");
            ViewBag.Action = "add";
            return View("UserForm");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewUser(User user)
        {
            if (ModelState.IsValid)
            {
                if(await _userService.GetUserByEmailAsync(user.Email) is { })
                {
                    ModelState.AddModelError("", "This Email is already registered");
                    var roles = await _roleService.GetAllAsync();
                    ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
                    ViewBag.Action = "add";
                    return View("UserForm");
                }
                user.Password = PasswordHelper.HashPassword(user.Password);
                await _userService.AddAsync(user);
            }
            else
            {
                ModelState.AddModelError("", "There is an invalid Data");
                var roles = await _roleService.GetAllAsync();
                ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
                ViewBag.Action = "add";
                return View("UserForm");
            }
            return RedirectToAction("Index","Dashboard");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(int? id)
        {
            if (id != null)
            {
                var user = await _userService.GetByIdAsync(id.Value);
                if (user != null)
                {
                    var roles = await _roleService.GetAllAsync();
                    ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
                    ViewBag.Action = "update";
                    return View("UserForm",user);
                }
            }
            return RedirectToAction("Index", "Dashboard");


        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var userData = await _userService.GetByIdAsync(user.UserId);
                userData.FullName = user.FullName;
                userData.Email = user.Email;
                userData.PhoneNumber = user.PhoneNumber;
                userData.BirthDate = user.BirthDate;
                userData.Gender = user.Gender;
                userData.RoleId = user.RoleId;
                await _userService.UpdateAsync(userData);
            }
            else
            {
                ModelState.AddModelError("", "There is an invalid Data");
                var roles = await _roleService.GetAllAsync();
                ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
                ViewBag.Action = "update";
                return View("UserForm");
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserPassword(int? id)
        {
            if (id != null)
            {
                var user = await _userService.GetByIdAsync(id.Value);
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserPassword(int UserId ,string Password)
        {
            var user = await _userService.GetByIdAsync(UserId);
            if (user != null)
            {
                user.Password = PasswordHelper.HashPassword(Password);
                await _userService.UpdateAsync(user);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        [Route("Dashboard/User/delete/{id?}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id is null) return RedirectToAction("index", "dashboard");
            var user = await _userService.GetByIdAsync(id.Value);
            if (user is { })
            {
                ViewBag.Name = user.FullName;
                ViewBag.Id = user.UserId;
                ViewBag.FieldName = "UserId";
                ViewBag.ActionName = user.RoleId == 2 ? "Doctors" : "Receptionists";
                return View("Delete");
            }

            return RedirectToAction("index", "dashboard");
        }

        [HttpPost]
        [Route("Dashboard/User/delete/{id?}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser()
        {
            var action = "";
            if (int.TryParse(Request.Form["UserId"], out int id))
            {
                var user = await _userService.GetByIdAsync(id);
                if (user is { })
                {
                    action = user.RoleId == 2 ? "Doctors" : "Receptionists";
                    await _userService.DeleteAsync(user);
                }
            }
            return RedirectToAction(action, "dashboard");

        }
    }
}
