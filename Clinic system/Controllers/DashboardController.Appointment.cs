using Clinic_system.Helpers;
using Clinic_system.Models;
using Clinic_system.Services;
using Clinic_system.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Appointments(DateTime? targetDate)
        {
            var date = targetDate is null ? DateTime.Today : targetDate;
            var appointments = await _appointmentService.GetAllAsync();
            appointments = appointments.Where(a => a.AppointmentDate.Date == date);
            ViewBag.Date = date;
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> CancelAppointment(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Dashboard");
            var appointment = await _appointmentService.GetByIdAsync(id.Value);
            if (appointment is { } && appointment.Status != AppointmentStatus.Cancelled)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                await _appointmentService.UpdateAsync(appointment);
            }
            var date = appointment?.AppointmentDate ?? DateTime.Today;
            return RedirectToAction("Appointments", "Dashboard", new { targetDate = date });
        }

        [HttpGet]
        public async Task<IActionResult> AddAppointment()
        {
            var services = await _servicesService.GetAllActiveServicesAsync();
            ViewBag.Services = new SelectList(services, "ServiceId", "ServiceName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var services = await _servicesService.GetAllActiveServicesAsync();
                ViewBag.Services = new SelectList(services, "ServiceId", "ServiceName");
                ModelState.AddModelError("", "البيانات غير صالحة");
                return View();
            }

            var patient = await _patientService.GetOrCreatePatientAsync(model);
            var appointment = await _appointmentService.CreateAppointmentAsync(model, patient);

            return RedirectToAction("Appointments", new { targetDate=appointment.AppointmentDate.Date });

        }
    }
}
