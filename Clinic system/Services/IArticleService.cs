using Clinic_system.Models;

namespace Clinic_system.Services
{
    public interface IArticleService : IGenericService<Article>
    {
        Task UpdateImage(Article article, IFormFile file);
        void DeleteImage(Article article);

    }
}
