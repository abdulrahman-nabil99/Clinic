using Clinic_system.Models;

namespace Clinic_system.Services
{
    public interface IArticleService : IGenericService<Article>
    {
        Task UpdateThumbnail(Article article, IFormFile img);
        void DeleteThumbnail(string img);

    }
}
