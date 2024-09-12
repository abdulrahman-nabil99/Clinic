using Clinic_system.Models;
using Clinic_system.Services;
using Clinic_system.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Clinic_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericService<Service> _servicesService;
        private readonly IPatientService _patientService;
        private readonly IGenericService<Appointment> _appointmentService;

        public HomeController(ILogger<HomeController> logger, IPatientService patientService, IGenericService<Service> servicesService, IGenericService<Appointment> appointmentService)
        {
            _logger = logger;
            _servicesService = servicesService;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Book()
        {
            var services = await _servicesService.GetAllAsync();
            ViewBag.Services = new SelectList(services, "ServiceId", "ServiceName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var patient = await _patientService.GetPatientByPhoneAsync(model.Phone);
                if (patient == null)
                {
                    patient = new Patient() 
                    { 
                        FullName = model.Name,
                        PhoneNumber=model.Phone,
                        Email = model.Email,
                    };
                    await _patientService.AddAsync(patient);
                }
                Appointment appointment = new Appointment()
                {
                    PatientId = patient.PatientId,
                    AppointmentDate = model.Date,
                    ServiceId = model.ServiceId,
                };
                await _appointmentService.AddAsync(appointment);
                ViewBag.Number = appointment.AppointmentId;
                var dayAppointments = await _appointmentService.GetAllAsync();
                ViewBag.Order = dayAppointments
                    .Where(a => a.AppointmentDate.Date == model.Date && a.Status== AppointmentStatus.Booked).Count();
                return View("SuccessBooking");
            }
            return RedirectToAction("Book","Home");
        }
    }
}
