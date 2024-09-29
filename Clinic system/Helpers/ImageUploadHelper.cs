using Clinic_system.Models;

namespace Clinic_system.Helpers
{
    public class ImageUploadHelper
    {
        public static async Task<string> UploadImage(IFormFile img, string location, string name)
        {
            string[] ext = { "jpg", "png", "jpeg", "gif" };
            var extension = img.FileName.Split('.').Last().ToLower();
            if (!ext.Contains(extension)) return string.Empty;
            var fileName = $"{name}.{extension}";
            using (FileStream file = new($"wwwroot/imgs/{location}/{fileName}", FileMode.Create))
            {
                await img.CopyToAsync(file);
            }
            return fileName;
        }
        public static void DeleteImage(string location, string img)
        {
            var filePath = Path.Combine($"wwwroot/imgs/{location}/", img);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
