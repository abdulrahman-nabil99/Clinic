using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinic_system.Controllers
{
    [Authorize]
    public partial class DashboardController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly IGenericService<Role> _roleService;
        private readonly IGenericService<MedicalRecord> _medicalRecordService;
        private readonly IArticleService _articleService;
        private readonly IGenericService<Inquiry> _inquiryService;
        private readonly IServiceService _servicesService;

        public DashboardController(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IUserService userService,
            IGenericService<Role> roleService,
            IGenericService<MedicalRecord> medicalRecordService,
            IArticleService articleService,
            IGenericService<Inquiry> inquiryService,
            IServiceService servicesService
            )
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _userService = userService;
            _roleService = roleService;
            _medicalRecordService = medicalRecordService;
            _articleService = articleService;
            _inquiryService = inquiryService;
            _servicesService = servicesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string s)
        {
            var res = await _patientService.Search(p=>p.PhoneNumber==s);
            if (res.Any())
            {
                return View("Patients", res);
            }
            res =  await _patientService.Search(p => p.FullName.ToLower().Contains(s.ToLower()));
            if (res.Any())
            {
                return View("Patients", res);
            }
            return RedirectToAction("Index");
        }

    }
}
