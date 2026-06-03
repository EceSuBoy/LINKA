using Linka.DtoLayer.IdentityDtos.UserDtos;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Linka.WebUI.Services
    .UserIdentityServices
{
    public class UserIdentityService
        : IUserIdentityService
    {
        private readonly HttpClient
            _httpClient;

        public UserIdentityService(
            HttpClient httpClient)
        {
            _httpClient =
                httpClient;
        }

        public async Task<
            PagedResultDto<
                ResultUserWithRoleDto>>
            GetAllUserListAsync(
                string? search,
                string? roleFilter,
                int page,
                int pageSize)
        {
            var encodedSearch =
                Uri.EscapeDataString(
                    search ??
                    string.Empty);

            var encodedRoleFilter =
                Uri.EscapeDataString(
                    roleFilter ??
                    "All");

            var requestUrl =
                "/api/users/GetAllUserList" +
                $"?search={encodedSearch}" +
                $"&roleFilter={encodedRoleFilter}" +
                $"&page={page}" +
                $"&pageSize={pageSize}";

            var responseMessage =
                await _httpClient
                    .GetAsync(
                        requestUrl);

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                throw new Exception(
                    "Users could not be loaded: " +
                    $"{responseMessage.StatusCode} - " +
                    $"{content}");
            }

            var values =
                JsonConvert
                    .DeserializeObject<
                        PagedResultDto<
                            ResultUserWithRoleDto>>(
                                content);

            return values ??
                new PagedResultDto<
                    ResultUserWithRoleDto>();
        }

        public async Task UpdateUserRoleAsync(
            UpdateUserRoleDto dto)
        {
            var responseMessage =
                await _httpClient
                    .PutAsJsonAsync(
                        "/api/users/UpdateUserRole",
                        dto);

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                throw new Exception(
                    "User role could not be updated: " +
                    $"{content}");
            }
        }
    }
}