using Clinic_system.Helpers;
using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
 
        public async Task<IActionResult> Patients(string? sort, string? sortedBy)
        {
            var patients = await _patientService.GetAllAsync();
            if (sort is not null)
            {
                if (sort == "id")
                {
                    // sorting
                    patients = SortHelper.Sort(patients, p => p.PatientId);
                    if (sortedBy is null)
                        ViewBag.SortedBy = sort;

                }
                else if (sort == "name")
                {
                    // sorting
                    patients = SortHelper.Sort(patients, p => p.FullName);
                    if (sortedBy is null)
                        ViewBag.SortedBy = sort;
                }

                if (sortedBy == sort)
                    patients = patients.Reverse();
            }
            return View(patients);
        }


    }
}
