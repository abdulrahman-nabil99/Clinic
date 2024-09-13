using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> PatientRecords(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Dashboard");
            var patient = await _patientService.GetByIdAsync(id.Value);
            if (patient is { })
            {
                var records = patient.MedicalHistory.ToList();
                ViewBag.PatientName = patient.FullName;
                return View(records);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> NewRecord(int? id)
        {
            if (id is null)
                return RedirectToAction("Index", "Dashboard");
            var appointment = await _appointmentService.GetByIdAsync(id.Value);
            if (appointment is { } && appointment.MedicalRecord is null)
            {
                ViewBag.Appointment = appointment;
                return View();
            }
            return RedirectToAction("Index", "Dashboard");
        }
        [HttpPost]
        public async Task<IActionResult> NewRecord(MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid && medicalRecord.AppointmentId is not null)
            {
                var appointment = await _appointmentService.GetByIdAsync(medicalRecord.AppointmentId.Value);
                if (appointment is { })
                {
                    appointment.MedicalRecord = medicalRecord;
                    appointment.Status = AppointmentStatus.Checked;
                    await _appointmentService.UpdateAsync(appointment);
                }
            }
            return RedirectToAction("PatientRecords", "Dashboard", new { id = medicalRecord.PatientId });
        }

    }
}
