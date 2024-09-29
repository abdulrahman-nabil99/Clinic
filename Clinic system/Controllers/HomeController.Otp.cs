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


        [HttpGet]
        public IActionResult TrackAppointments()
        {
            var hasAccess = HttpContext.Session.GetString("HasAccess") == "true";
            var phone = HttpContext.Session.GetString("Phone");

            if (hasAccess && phone is not null)
                return RedirectToAction("MyAppointments", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TrackAppointments(TrackViewModel model)
        {
            if (!ModelState.IsValid ||
                await _patientService.GetPatientByPhoneAsync(model.PhoneNumber) is not { } patient ||
                patient.Email != model.EmailAddress)
            {
                ModelState.AddModelError("", "«·»Ì«‰«  €Ì— ’«·Õ…");
                return View();
            }

            // Generate Otp and save it in cache
            var otp = _otpHelper.GenerateOtp(model.PhoneNumber);

            // Send Otp to email 
            string body = $"<h2>—ﬁ„  √ﬂÌœ «·œŒÊ· ·„ «»⁄… ÕÃÊ“« ﬂ</h2>" +
                $"<p>” ‰ ÂÌ ’·«ÕÌ… «·—ﬁ„ »⁄œ „—Ê— 30 œﬁÌﬁÌ…</p>" +
                $"<p>—ﬁ„ «· √ﬂÌœ : {otp}</p>";
            await EmailHelper.SendEmailAsync(patient.Email, "—ﬁ„  √ﬂÌœ «·œŒÊ·", body);
            return View("VerifyOtp", model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(TrackViewModel model)
        {
            if (!ModelState.IsValid ||
                await _patientService.GetPatientByPhoneAsync(model.PhoneNumber) is not { } patient)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!_otpHelper.VerifyOtp(model.PhoneNumber, model.Otp))
            {
                ModelState.AddModelError("", "—ﬁ„ «· √ﬂÌœ €Ì— ’ÕÌÕ");
                return View("VerifyOtp", model);
            }
            HttpContext.Session.SetString("HasAccess", "true");
            HttpContext.Session.SetString("Phone", model.PhoneNumber);
            return RedirectToAction("MyAppointments", "Home");
        }


    }
}