using Clinic_system.Helpers;
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
        private readonly IAppointmentService _appointmentService;
        private readonly IGenericService<Inquiry> _inquiryService;
        private readonly GenericHelpers _genericHelpers;

        public HomeController(
            ILogger<HomeController> logger,
            IPatientService patientService,
            IGenericService<Service> servicesService,
            IAppointmentService appointmentService,
            IGenericService<Inquiry> inquiryService,
            GenericHelpers genericHelpers
            )
        {
            _logger = logger;
            _servicesService = servicesService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _inquiryService = inquiryService;
            _genericHelpers = genericHelpers;
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
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Book", "Home");
            }

            if (_genericHelpers.HasExceededRequestLimit("Book",HttpContext))
            {
                return BadRequest("You have reached the limit for sending inquiries for today.");
            }

            var patient = await _patientService.GetOrCreatePatientAsync(model);
            var appointment = await _appointmentService.CreateAppointmentAsync(model, patient);

            ViewBag.Number = appointment.AppointmentId;
            ViewBag.Order = await _appointmentService.GetAppointmentOrderForDayAsync(model.Date);

            return View("SuccessBooking");
        }

        [HttpPost]
        public async Task<IActionResult> NewInquiry(Inquiry inquiry)
        {
            if (_genericHelpers.HasExceededRequestLimit("Inquiry",HttpContext))
            {
                return BadRequest("You have reached the limit for sending inquiries for today.");
            }
            if (ModelState.IsValid)
            {
                await _inquiryService.AddAsync(inquiry);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
