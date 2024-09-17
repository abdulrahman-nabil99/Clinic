using Clinic_system.Helpers;
using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
        public async Task<IActionResult> Inquiries()
        {
            var inquiries = await _inquiryService.GetAllAsync();
            return View(inquiries);
        }

        [HttpGet]
        [Route("Dashboard/Inquiry/Answer/{id?}")]
        public async Task<IActionResult> AddInquiryAnswer(int? id)
        {
            if (id is null || await _inquiryService.GetByIdAsync(id.Value) is not { } inquiry || inquiry.IsAnswered)
            {
                return RedirectToAction("Inquiries", "Dashboard");
            }

            return View(inquiry);
        }

        [HttpPost]
        [Route("Dashboard/Inquiry/Answer/{id?}")]
        public async Task<IActionResult> AddInquiryAnswer(Inquiry model)
        {
            if (model is not null && await _inquiryService.GetByIdAsync(model.Id) is { } inquiry && !inquiry.IsAnswered && model.Answer?.Trim().Length>=1)
            {
                inquiry.IsAnswered = true;
                inquiry.Answer = model.Answer;
                await _inquiryService.UpdateAsync(inquiry);
                string body = $"<h2>{inquiry.Question}</h2><p>{inquiry.Answer}</p>";
                await EmailHelper.SendEmailAsync(inquiry.EmailAddress, "اجابة استفساراتكم", body);
            }
            
            return RedirectToAction("Inquiries", "Dashboard");
        }

        [Route("Dashboard/Inquiry/Delete/{id?}")]
        [HttpGet]
        public async Task<IActionResult> DeleteInquiry(int? id)
        {
            if (id == null || (await _inquiryService.GetByIdAsync(id.Value) is not { } inquiry))
            {
                return RedirectToAction("index", "dashboard");
            }

            ViewBag.Name = $"إستفسار {inquiry.Name}";
            ViewBag.Id = inquiry.Id;
            ViewBag.FieldName = "Id";
            ViewBag.ActionName = "Inquiries";

            return View("Delete");
        }

        [Route("Dashboard/Inquiry/Delete/{id?}")]
        [HttpPost]
        public async Task<IActionResult> DeleteInquiry()
        {
            if (int.TryParse(Request.Form["Id"], out int id))
            {
                var inquiry = await _inquiryService.GetByIdAsync(id);
                if (inquiry is { })
                {
                    await _inquiryService.DeleteAsync(inquiry);
                }
            }
            return RedirectToAction("Inquiries", "Dashboard");

        }
    }
}
