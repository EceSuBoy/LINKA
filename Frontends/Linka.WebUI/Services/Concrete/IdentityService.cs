using IdentityModel.Client;
using Linka.DtoLayer.IdentityDtos.LoginDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Linka.WebUI.Services.Concrete
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<bool> GetRefreshToken()
        {
            var discoveryEndPoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = _clientSettings.LinkaManagerClient.ClientId,
                ClientSecret = _clientSettings.LinkaManagerClient.ClientSecret,
                RefreshToken = refreshToken,
                Address = discoveryEndPoint.TokenEndpoint
            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            var authenticationToken= new List<AuthenticationToken>()
            {

                    new AuthenticationToken{
                        Name = OpenIdConnectParameterNames.AccessToken,
                        Value = token.AccessToken
                    },


            new AuthenticationToken
            {
                Name=OpenIdConnectParameterNames.RefreshToken,
                Value= token.RefreshToken
            },

            new AuthenticationToken
            {
                Name=OpenIdConnectParameterNames.ExpiresIn,
                Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", System.Globalization.CultureInfo.InvariantCulture)
            }
            };
            var result = await _httpContextAccessor.HttpContext.AuthenticateAsync();

            var properties = result.Properties;
            properties.StoreTokens(authenticationToken);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Principal, properties);


            return true;

        }
        

        public async Task<bool> SignIn(SignInDto signInDto)
        {
            var discoveryEndPoint= await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityServerUrl,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            var passwordTokenRequest =
    new PasswordTokenRequest
    {
        ClientId =
            _clientSettings
                .LinkaManagerClient
                .ClientId,

        ClientSecret =
            _clientSettings
                .LinkaManagerClient
                .ClientSecret,

        UserName =
            signInDto.Username,

        Password =
            signInDto.Password,

        Address =
            discoveryEndPoint
                .TokenEndpoint,

        Scope =
            "openid profile email " +
            "IdentityServerApi " +
            "CatalogFullPermission " +
            "CatalogReadPermission " +
            "DiscountFullPermission " +
            "OrderFullPermission " +
            "CargoFullPermission " +
            "BasketFullPermission " +
            "CommentFullPermission " +
            "PaymentFullPermission " +
            "ImagesFullPermission " +
            "OcelotFullPermission " +
            "MessageFullPermission"
    };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
            if (token.IsError)
            {
                throw new Exception($"Token Error: {token.Error} - {token.ErrorDescription}");
            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = discoveryEndPoint.UserInfoEndpoint      
            };

            var userValues =
    await _httpClient
        .GetUserInfoAsync(
            userInfoRequest);

            using var currentUserRequest =
                new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{_serviceApiSettings.IdentityServerUrl}" +
                    "/api/users/GetUser");

            currentUserRequest
                .Headers
                .Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token.AccessToken);

            using var currentUserResponse =
                await _httpClient
                    .SendAsync(
                        currentUserRequest);

            if (!currentUserResponse.IsSuccessStatusCode)
            {
                var errorContent =
                    await currentUserResponse
                        .Content
                        .ReadAsStringAsync();

                throw new Exception(
                    "User role information could not be retrieved: " +
                    $"{currentUserResponse.StatusCode} - " +
                    $"{errorContent}");
            }

            var currentUser =
                await currentUserResponse
                    .Content
                    .ReadFromJsonAsync<
                        UserDetailViewModel>();

            var claims =
                userValues.Claims
                    .ToList();

            foreach (
                var role in
                currentUser?.Roles ??
                new List<string>())
            {
                var roleAlreadyExists =
                    claims.Any(
                        x =>
                            x.Type == "role" &&
                            x.Value == role);

                if (!roleAlreadyExists)
                {
                    claims.Add(
                        new Claim(
                            "role",
                            role));
                }
            }

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme,
                    "name",
                    "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            
                authenticationProperties.StoreTokens(new List<AuthenticationToken>()
                {
                    new AuthenticationToken{
                        Name = OpenIdConnectParameterNames.AccessToken,
                        Value = token.AccessToken
                    },
                
                
            new AuthenticationToken
            {
                Name=OpenIdConnectParameterNames.RefreshToken,
                Value= token.RefreshToken
            },

            new AuthenticationToken
            {
                Name=OpenIdConnectParameterNames.ExpiresIn,
                Value= DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o", System.Globalization.CultureInfo.InvariantCulture)
            }
            });

            authenticationProperties.IsPersistent = false;

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return true;

        }
    }
}
