using Clinic_system.Helpers;
using Clinic_system.Models;
using Clinic_system.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_system.Controllers
{
    public partial class DashboardController : Controller
    {
        public async Task<IActionResult> Articles()
        {
            var articles = await _articleService.GetAllAsync();
            return View(articles);
        }
        [HttpGet]
        public IActionResult NewArticle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewArticle(Article article, IFormFile? img)
        {
            if (ModelState.IsValid)
            {
                await _articleService.AddAsync(article);
                if (img != null)
                    await _articleService.UpdateImage(article, img);
            }
            return RedirectToAction("Articles", "Dashboard");
        }

        [Route("Dashboard/Article/Update/{id?}")]
        [HttpGet]
        public async Task<IActionResult> UpdateArticle(int? id)
        {
            if (id is null || await _articleService.GetByIdAsync(id.Value) is not { } article)
            {
                return RedirectToAction("Articles", "Dashboard");
            }

            return View("NewArticle", article);
        }

        [Route("Dashboard/Article/Update/{id?}")]
        [HttpPost]
        public async Task<IActionResult> UpdateArticle(Article article, IFormFile? img)
        {
            if (ModelState.IsValid)
            {
                await _articleService.UpdateAsync(article);
                if (img != null)
                    await _articleService.UpdateImage(article, img);
            }
            return RedirectToAction("Articles", "Dashboard");
        }

        [Route("Dashboard/Article/Delete/{id?}")]
        [HttpGet]
        public async Task<IActionResult> DeleteArticle(int? id)
        {
            if (id == null || (await _articleService.GetByIdAsync(id.Value) is not { } article))
            {
                return RedirectToAction("index", "dashboard");
            }

            ViewBag.Name = article.Title;
            ViewBag.Id = article.Id;
            ViewBag.FieldName = "Id";
            ViewBag.ActionName = "Articles";

            return View("Delete");
        }

        [Route("Dashboard/Article/Delete/{id?}")]
        [HttpPost]
        public async Task<IActionResult> DeleteArticle()
        {
            if (int.TryParse(Request.Form["Id"], out int id))
            {
                var article = await _articleService.GetByIdAsync(id);
                if (article is { })
                {
                    if (article.Thumbnail != "default.png")
                        _articleService.DeleteImage(article);
                    await _articleService.DeleteAsync(article);
                }
            }
            return RedirectToAction("Articles", "dashboard");

        }
    }
}
