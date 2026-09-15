namespace Linka.Images.Services
{
    public interface ICloudStorageService
    {
        Task<string> UploadFileAsync(
            IFormFile fileToUpload,
            string fileNameToSave);

        Task<string> GetSignedUrlAsync(
            string fileNameToRead,
            int timeOutInMinutes = 30);

        Task DeleteFileAsync(
            string fileNameToDelete);
    }
}