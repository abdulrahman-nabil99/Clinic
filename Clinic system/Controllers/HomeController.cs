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
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericService<Service> _servicesService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IGenericService<Inquiry> _inquiryService;
        private readonly RateLimiterHelper _rateLimiterHelper;
        private readonly OtpHelper _otpHelper;

        public HomeController(
            ILogger<HomeController> logger,
            IPatientService patientService,
            IGenericService<Service> servicesService,
            IAppointmentService appointmentService,
            IGenericService<Inquiry> inquiryService,
            RateLimiterHelper rateLimiterHelper,
            OtpHelper otpHelper
            )
        {
            _logger = logger;
            _servicesService = servicesService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _inquiryService = inquiryService;
            _rateLimiterHelper = rateLimiterHelper;
            _otpHelper = otpHelper;
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

            if (_rateLimiterHelper.HasExceededRequestLimit("Book", HttpContext))
            {
                return BadRequest("You have reached the limit for sending inquiries for today.");
            }

            var patient = await _patientService.GetOrCreatePatientAsync(model);
            var appointment = await _appointmentService.CreateAppointmentAsync(model, patient);

            var order = await _appointmentService.GetAppointmentOrderForDayAsync(model.Date);

            //Send booking email
            try
            {
                string body = $"<h2> „  ”ÃÌ· ÕÃ“ﬂ »‰Ã«Õ</h2>" +
                    $"<p>—ﬁ„ ÕÃ“ﬂ ÂÊ : {appointment.AppointmentId}</p>" +
                    $"<p> — Ì»ﬂ : {order}</p>";
                await EmailHelper.SendEmailAsync(model.Email, " „  √ﬂÌœ «·ÕÃ“", body);
                ViewBag.Message = " „  ”ÃÌ· «·ÕÃ“ »‰Ã«Õ,  „ ≈—”«· »Ì«‰«  «·ÕÃ“ ≈·Ï «·≈Ì„Ì· «·Œ«’ »ﬂ";
            }
            catch
            {
                ViewBag.Message = "«·«Ì„Ì· «·„œŒ· €Ì— ’ÕÌÕ";
            }

            return View("SuccessBooking");
        }

        [HttpPost]
        public async Task<IActionResult> NewInquiry(Inquiry inquiry)
        {
            if (_rateLimiterHelper.HasExceededRequestLimit("Inquiry", HttpContext))
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