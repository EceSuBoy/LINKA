using Linka.DtoLayer.IdentityDtos.LoginDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services;
using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Linka.WebUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IIdentityService _identityService;

        public LoginController(IHttpClientFactory httpClientFactory, IIdentityService identityService)
        {
            _httpClientFactory = httpClientFactory;
            _identityService = identityService;
        }

        [HttpGet]

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(CreateLoginDto createLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createLoginDto);
            }

            var signInDto =
                new SignInDto
                {
                    Username =
                        createLoginDto.Username,

                    Password =
                        createLoginDto.Password
                };

            try
            {
                await _identityService
                    .SignIn(signInDto);

                return RedirectToAction(
                    "Index",
                    "Default");
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("invalid_grant") ||
                    exception.Message.Contains("invalid_username_or_password") ||
                    exception.Message.Contains("Username or password"))
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Username or password is incorrect.");
                }
                else
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Login failed. Please try again later.");
                }

                return View(createLoginDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Default");
        }
    }
}
