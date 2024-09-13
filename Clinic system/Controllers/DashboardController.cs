using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IGenericService<Role> _roleService;
        private readonly IGenericService<MedicalRecord> _medicalRecordService;
        private readonly IArticleService _articleService;
        private readonly IGenericService<Inquiry> _inquiryService;

        public DashboardController(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IUserService userService,
            IGenericService<Role> roleService,
            IGenericService<MedicalRecord> medicalRecordService,
            IArticleService articleService,
            IGenericService<Inquiry> inquiryService
            )
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _userService = userService;
            _roleService = roleService;
            _medicalRecordService = medicalRecordService;
            _articleService = articleService;
            _inquiryService = inquiryService;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
