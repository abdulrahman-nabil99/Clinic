using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IGenericService<Appointment> _appointmentService;
        private readonly IUserService _userService;
        private readonly IGenericService<Role> _roleService;
        private readonly IGenericService<MedicalRecord> _medicalRecordService;
        private readonly IArticleService _articleService;

        public DashboardController(
            IPatientService patientService,
            IGenericService<Appointment> appointmentService,
            IUserService userService,
            IGenericService<Role> roleService,
            IGenericService<MedicalRecord> medicalRecordService,
            IArticleService articleService
            )
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _userService = userService;
            _roleService = roleService;
            _medicalRecordService = medicalRecordService;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Patients()
        {
            var patients = await _patientService.GetAllAsync();
            return View(patients);
        }

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
            if (appointment is { } && appointment.MedicalRecordId is null)
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
