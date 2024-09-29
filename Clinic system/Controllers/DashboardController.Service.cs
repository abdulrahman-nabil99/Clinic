
using Clinic_system.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
        public async Task<IActionResult> Services()
        {
            var services = await _servicesService.GetAllAsync();
            return View(services);
        }

        public IActionResult NewService()
        {
            ViewBag.Action = "add";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewService(Service service, IFormFile? img)
        {
            if (ModelState.IsValid)
            {
                await _servicesService.AddAsync(service);
                if (img != null)
                    await _servicesService.UpdateImage(service,img);
            }
            return RedirectToAction("Services", "Dashboard");
        }

        [Route("Dashboard/Service/Update/{id?}")]
        [HttpGet]
        public async Task<IActionResult> UpdateService(int? id)
        {
            if (id is null || await _servicesService.GetByIdAsync(id.Value) is not { } service)
            {
                return RedirectToAction("Services", "Dashboard");
            }
            ViewBag.Action = "update";

            return View("NewService", service);
        }

        [Route("Dashboard/Service/Update/{id?}")]
        [HttpPost]
        public async Task<IActionResult> UpdateService(Service service, IFormFile? img)
        {
            if (ModelState.IsValid)
            {
                await _servicesService.UpdateAsync(service);
                if (img != null)
                    await _servicesService.UpdateImage(service, img);
            }
            return RedirectToAction("Services", "Dashboard");
        }

        [Route("Dashboard/Service/Delete/{id?}")]
        [HttpGet]
        public async Task<IActionResult> DeleteService(int? id)
        {
            if (id == null || (await _servicesService.GetByIdAsync(id.Value) is not { } service))
            {
                return RedirectToAction("index", "dashboard");
            }

            ViewBag.Name = service.ServiceName;
            ViewBag.Id = service.ServiceId;
            ViewBag.FieldName = "ServiceId";
            ViewBag.ActionName = "Services";

            return View("Delete");
        }

        [Route("Dashboard/Service/Delete/{id?}")]
        [HttpPost]
        public async Task<IActionResult> DeleteService()
        {
            if (int.TryParse(Request.Form["ServiceId"], out int id))
            {
                var service = await _servicesService.GetByIdAsync(id);
                if (service is { })
                {
                    if (!service.ServiceImage.Contains("default"))
                        _servicesService.DeleteImage(service);
                    await _servicesService.DeleteAsync(service);
                }
            }
            return RedirectToAction("Services", "dashboard");

        }
    }
}
