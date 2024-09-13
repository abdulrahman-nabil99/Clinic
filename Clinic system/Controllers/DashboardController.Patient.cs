using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
 
        public async Task<IActionResult> Patients()
        {
            var patients = await _patientService.GetAllAsync();
            return View(patients);
        }


    }
}
