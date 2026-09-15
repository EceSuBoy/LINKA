namespace Linka.WebUI.Services.ImageUploadServices
{
    public interface IImageUploadService
    {
        Task<string?> UploadAsync(
            IFormFile file);

        Task DeleteAsync(
            string fileName);
    }
}