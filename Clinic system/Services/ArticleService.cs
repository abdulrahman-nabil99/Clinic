using Clinic_system.Data;
using Clinic_system.Models;

namespace Clinic_system.Services
{
    public class ArticleService : GenericService<Article>, IArticleService
    {
        public ArticleService(ClinicdbContext _context) : base(_context)
        {
        }

        public async Task UpdateThumbnail(Article article,IFormFile img)
        {
            string[] ext = { "jpg", "png", "jpeg", "gif" };
            var extension = img.FileName.Split('.').Last().ToLower();
            if (!ext.Contains(extension)) return;
            var fileName = $"article-{article.Id}-thumbnail.{extension}";
            if (article.Thumbnail != "default.png")
            {
                DeleteThumbnail(article.Thumbnail);
            }
            using (FileStream file = new($"wwwroot/imgs/thumbnails/{fileName}", FileMode.Create))
            {
                await img.CopyToAsync(file);
            }
            article.Thumbnail = fileName;
            await UpdateAsync(article);
            return;
        }
        public void DeleteThumbnail(string img)
        {
            var filePath = Path.Combine("wwwroot/imgs/thumbnails/", img);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return;
        }
    }
}
