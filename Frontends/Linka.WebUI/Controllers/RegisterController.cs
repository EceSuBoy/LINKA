using Linka.DtoLayer.IdentityDtos.RegisterDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Linka.WebUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory
            _httpClientFactory;

        public RegisterController(
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory =
                httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(
            CreateRegisterDto createRegisterDto)
        {
            if (createRegisterDto.Password !=
                createRegisterDto.ConfirmPassword)
            {
                ModelState.AddModelError(
                    nameof(
                        createRegisterDto
                            .ConfirmPassword),

                    "Passwords do not match.");

                return View(
                    createRegisterDto);
            }

            var client =
                _httpClientFactory
                    .CreateClient();

            var jsonData =
                JsonConvert.SerializeObject(
                    createRegisterDto);

            var stringContent =
                new StringContent(
                    jsonData,
                    Encoding.UTF8,
                    "application/json");

            var responseMessage =
                await client.PostAsync(
                    "http://localhost:5001/api/Registers",
                    stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction(
                    "Index",
                    "Login");
            }

            var errorContent =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            var errors =
                JsonConvert
                    .DeserializeObject<
                        List<string>>(
                            errorContent);

            if (errors != null &&
                errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error);
                }
            }
            else
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Registration failed. Please check your information and try again.");
            }

            return View(
                createRegisterDto);
        }
    }
}