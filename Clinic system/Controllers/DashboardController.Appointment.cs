using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

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

    }
}
