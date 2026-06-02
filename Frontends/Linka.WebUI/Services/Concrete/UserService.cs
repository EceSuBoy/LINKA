using Linka.WebUI.Models;
using Linka.WebUI.Services.Interfaces;

namespace Linka.WebUI.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDetailViewModel> GetUserInfo()
        {
            var user =
                await _httpClient
                    .GetFromJsonAsync<UserDetailViewModel>(
                        "/api/users/getuser");

            if (user == null)
            {
                throw new Exception(
                    "Current user information could not be loaded.");
            }

            return user;
        }

        public async Task<UserDetailViewModel>
            GetUserByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException(
                    "User ID cannot be empty.");
            }

            var responseMessage =
                await _httpClient.GetAsync(
                    "/api/users/getuserbyid/" +
                    Uri.EscapeDataString(id));

            if (!responseMessage.IsSuccessStatusCode)
            {
                var content =
                    await responseMessage.Content
                        .ReadAsStringAsync();

                throw new Exception(
                    $"User information could not be loaded: " +
                    $"{responseMessage.StatusCode} - {content}");
            }

            var user =
                await responseMessage.Content
                    .ReadFromJsonAsync<UserDetailViewModel>();

            if (user == null)
            {
                throw new Exception(
                    "IdentityServer returned an empty user response.");
            }

            return user;
        }
    }
}