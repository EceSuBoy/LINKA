using System.Net.Http.Headers;
using System.Text.Json;

namespace Linka.WebUI.Services.ImageUploadServices
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly HttpClient _httpClient;

        // BaseAddress is set in Program.cs to: {OcelotUrl}/{Images.Path}  ->  http://localhost:5000/services/images/
        public ImageUploadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            // field name must be "file" — matches FilesController.Upload(IFormFile file)
            content.Add(fileContent, "file", file.FileName);

            var response = await _httpClient.PostAsync("files/upload", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            // Prefer the clean public URL; fall back to the GCS media link.
            var root = doc.RootElement;
            if (root.TryGetProperty("url", out var url) && url.GetString() is { Length: > 0 } u)
                return u;
            if (root.TryGetProperty("mediaLink", out var media))
                return media.GetString();

            return null;
        }

        public async Task DeleteAsync(string fileName)
        {
            await _httpClient.DeleteAsync($"files/{fileName}");
        }
    }
}
