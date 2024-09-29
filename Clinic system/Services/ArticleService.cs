using Clinic_system.Data;
using Clinic_system.Helpers;
using Clinic_system.Models;

namespace Clinic_system.Services
{
    public class ArticleService : GenericService<Article>, IArticleService
    {
        public ArticleService(ClinicdbContext _context) : base(_context)
        {
        }

        public async Task UpdateImage(Article article, IFormFile file)
        {
            if (!article.Thumbnail.Contains("default"))
                DeleteImage(article);
            var image = await ImageUploadHelper
                .UploadImage(file, "thumbnails", $"article-{article.Id}-thumbnail");
            article.Thumbnail = image;
            await UpdateAsync(article);
        }

        public void DeleteImage(Article article)
        {
            ImageUploadHelper.DeleteImage("thumbnails", article.Thumbnail);
        }

    }
}
