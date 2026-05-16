using IdentityModel.Client;
using Linka.DtoLayer.IdentityDtos.LoginDtos;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Settings;
using Microsoft.Extensions.Options;

namespace Linka.WebUI.Services.Concrete
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
        }

        public async Task<bool> SignIn(SignUpDto signUpDto)
        {
            var discoveryEndPoint= await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://localhost:5001",
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.LinkaManagerId.ClientId,
                ClientSecret = _clientSettings.LinkaManagerId.ClientSecret,
                UserName = signUpDto.Username,
                Password = signUpDto.Password,
                Address= discoveryEndPoint.TokenEndpoint
            };
        }
    }
}
