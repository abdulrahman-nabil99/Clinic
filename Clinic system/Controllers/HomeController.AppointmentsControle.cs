using Clinic_system.Helpers;
using Clinic_system.Models;
using Clinic_system.Services;
using Clinic_system.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Clinic_system.Controllers
{
    public partial class HomeController : Controller
    {
        [Route("MyAppointments")]
        public async Task<IActionResult> MyAppointments()
        {
            var hasAccess = HttpContext.Session.GetString("HasAccess") == "true";
            var phone = HttpContext.Session.GetString("Phone");

            if (!hasAccess || phone is null)
                return RedirectToAction("Index", "Home");

            var patient = await _patientService.GetPatientByPhoneAsync(phone);

            if (patient is null)
                return RedirectToAction("Index", "Home");

            var appointment = patient.Appointments.ToList();
            return View(appointment);
        }

        [Route("MyAppointments/Update/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateMyAppointment(int? id)
        {
            var hasAccess = HttpContext.Session.GetString("HasAccess") == "true";
            var phone = HttpContext.Session.GetString("Phone");

            if (!hasAccess || phone is null || id is null)
                return RedirectToAction("Index", "Home");

            var patient = await _patientService.GetPatientByPhoneAsync(phone);
            var appointment = await _appointmentService.GetByIdAsync(id.Value);
            if (appointment is null || appointment.PatientId !=patient.PatientId)
                return RedirectToAction("Index", "Home");

            var services = await _servicesService.GetAllAsync();
            ViewBag.Services = new SelectList(services, "ServiceId", "ServiceName");

            var model = new BookViewModel()
            {
                Name = patient.FullName,
                Email = patient.Email ?? "No Email Registered",
                Phone = patient.PhoneNumber,
                Date = appointment.AppointmentDate,
                ServiceId = appointment.ServiceId is null ? 0 : appointment.ServiceId.Value,
            };
            return View(model);
        }

        [Route("MyAppointments/Update/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateMyAppointment(int? id, BookViewModel model)
        {
            var hasAccess = HttpContext.Session.GetString("HasAccess") == "true";
            var phone = HttpContext.Session.GetString("Phone");

            if (!hasAccess || phone is null || id is null)
                return RedirectToAction("Index", "Home");
            
            var patient = await _patientService.GetPatientByPhoneAsync(phone);
            var appointment = await _appointmentService.GetByIdAsync(id.Value);
            if (appointment is null || appointment.PatientId != patient.PatientId)
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                appointment.ServiceId = model.ServiceId;
                appointment.AppointmentDate = model.Date;
                await _appointmentService.UpdateAsync(appointment);
            }
            return RedirectToAction("MyAppointments","Home");
        }

        [Route("MyAppointments/Cancel/{id}")]
        [HttpGet]
        public async Task<IActionResult> CancelMyAppointment(int? id)
        {
            var hasAccess = HttpContext.Session.GetString("HasAccess") == "true";
            var phone = HttpContext.Session.GetString("Phone");

            if (!hasAccess || phone is null || id is null)
                return RedirectToAction("Index", "Home");

            var patient = await _patientService.GetPatientByPhoneAsync(phone);
            var appointment = await _appointmentService.GetByIdAsync(id.Value);
            if (appointment is null || appointment.PatientId != patient.PatientId)
                return RedirectToAction("Index", "Home");

            ViewBag.AppointmentNumber = appointment.AppointmentId;
            return View();
        }

        [Route("MyAppointments/Cancel/{id}")]
        [HttpPost]
        public async Task<IActionResult> CancelMyAppointment()
        {
            var hasAccess = HttpContext.Session.GetString("HasAccess") == "true";
            var phone = HttpContext.Session.GetString("Phone");

            if (!hasAccess || phone is null || !int.TryParse(Request.Form["AppointmentId"], out int id))
                return RedirectToAction("Index", "Home");

            var patient = await _patientService.GetPatientByPhoneAsync(phone);
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment is null || appointment.PatientId != patient.PatientId)
                return RedirectToAction("Index", "Home");

            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentService.UpdateAsync(appointment);

            return RedirectToAction("MyAppointments", "Home");
        }

        [Route("MyAppointments/Appointment/{id}")]
        public async Task<IActionResult> MyRecords(int? id)
        {
            var hasAccess = HttpContext.Session.GetString("HasAccess") == "true";
            var phone = HttpContext.Session.GetString("Phone");

            if (!hasAccess || phone is null || id is null)
                return RedirectToAction("Index", "Home");

            var patient = await _patientService.GetPatientByPhoneAsync(phone);
            var appointment = await _appointmentService.GetByIdAsync(id.Value);
            if (appointment is null || appointment.PatientId != patient.PatientId || appointment.Status != AppointmentStatus.Checked)
                return RedirectToAction("Index", "Home");

            var record = appointment.MedicalRecord;
            return View(record);
        }
    }
}