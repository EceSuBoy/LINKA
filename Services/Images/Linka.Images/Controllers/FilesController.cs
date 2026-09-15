using Linka.Images.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Linka.Images.Controllers
{
    /// <summary>
    /// File API for the cloud bucket.
    ///
    /// Gateway maps:  /services/images/{everything}  ->  /api/{everything}
    /// So this controller (route "api/files") is reachable through Ocelot at:
    ///   POST    {gateway}/services/images/files/upload
    ///   DELETE  {gateway}/services/images/files/{fileName}
    ///   GET     {gateway}/services/images/files/signed/{fileName}
    /// </summary>
    [AllowAnonymous] // tighten later (see guide) — open while wiring it up
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ICloudStorageService _cloudStorage;
        private readonly GCSConfigOptions _options;

        public FilesController(ICloudStorageService cloudStorage, IOptions<GCSConfigOptions> options)
        {
            _cloudStorage = cloudStorage;
            _options = options.Value;
        }

        // POST /api/files/upload   (multipart/form-data, field name: "file")
        [HttpPost("upload")]
        [RequestSizeLimit(20_000_000)] // 20 MB
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was provided.");

            // Unique, safe file name so two uploads never collide.
            var ext = Path.GetExtension(file.FileName);
            var baseName = Path.GetFileNameWithoutExtension(file.FileName);
            var safeName = $"{baseName}-{DateTime.UtcNow:yyyyMMddHHmmssfff}{ext}";

            var mediaLink = await _cloudStorage.UploadFileAsync(file, safeName);

            // If the bucket (or this object) is public, this is a clean permanent URL.
            var publicUrl = $"https://storage.googleapis.com/{_options.GoogleCloudStorageBucketName}/{safeName}";

            return Ok(new
            {
                fileName = safeName,
                url = publicUrl,   // use this for product images (needs public bucket)
                mediaLink          // authenticated link returned by GCS
            });
        }

        // DELETE /api/files/{fileName}
        [HttpDelete("{fileName}")]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _cloudStorage.DeleteFileAsync(fileName);
            return Ok(new { deleted = fileName });
        }

        // GET /api/files/signed/{fileName}?minutes=30   (temporary private read link)
        [HttpGet("signed/{fileName}")]
        public async Task<IActionResult> Signed(string fileName, int minutes = 30)
        {
            var url = await _cloudStorage.GetSignedUrlAsync(fileName, minutes);
            return Ok(new { fileName, signedUrl = url, expiresInMinutes = minutes });
        }
    }
}
