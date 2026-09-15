using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace Linka.Images.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly GCSConfigOptions _options;
        private readonly ILogger<CloudStorageService> _logger;
        private readonly GoogleCredential _googleCredential;

        public CloudStorageService(IOptions<GCSConfigOptions> options, ILogger<CloudStorageService> logger)
        {
            _options = options.Value;
            _logger = logger;

            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                _googleCredential = environment == Environments.Production
                    ? GoogleCredential.FromJson(_options.GCPStorageAuthFile)
                    : GoogleCredential.FromFile(_options.GCPStorageAuthFile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<string> UploadFileAsync(
    IFormFile fileToUpload,
    string fileNameToSave)
        {
            if (fileToUpload == null ||
                fileToUpload.Length == 0)
            {
                throw new ArgumentException(
                    "File cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(
                    fileToUpload.ContentType) ||
                !fileToUpload.ContentType
                    .StartsWith("image/"))
            {
                throw new ArgumentException(
                    "Only image files can be uploaded.");
            }

            using var memoryStream =
                new MemoryStream();

            await fileToUpload.CopyToAsync(
                memoryStream);

            memoryStream.Position = 0;

            using var storageClient =
                StorageClient.Create(
                    _googleCredential);

            await storageClient.UploadObjectAsync(
                _options.GoogleCloudStorageBucketName,
                fileNameToSave,
                fileToUpload.ContentType,
                memoryStream);

            var publicUrl =
                $"https://storage.googleapis.com/{_options.GoogleCloudStorageBucketName}/{Uri.EscapeDataString(fileNameToSave)}";

            _logger.LogInformation(
                "Uploaded {FileName} to {BucketName}. Url: {Url}",
                fileNameToSave,
                _options.GoogleCloudStorageBucketName,
                publicUrl);

            return publicUrl;
        }

        public async Task<string> GetSignedUrlAsync(string fileNameToRead, int timeOutInMinutes = 30)
        {
            var sac = _googleCredential.UnderlyingCredential as ServiceAccountCredential;
            var urlSigner = UrlSigner.FromServiceAccountCredential(sac);

            var signedUrl = await urlSigner.SignAsync(
                _options.GoogleCloudStorageBucketName,
                fileNameToRead,
                TimeSpan.FromMinutes(timeOutInMinutes));

            return signedUrl.ToString();
        }

        public async Task DeleteFileAsync(string fileNameToDelete)
        {
            using var storageClient = StorageClient.Create(_googleCredential);
            await storageClient.DeleteObjectAsync(_options.GoogleCloudStorageBucketName, fileNameToDelete);
            _logger.LogInformation($"Deleted {fileNameToDelete}");
        }
    }
}
