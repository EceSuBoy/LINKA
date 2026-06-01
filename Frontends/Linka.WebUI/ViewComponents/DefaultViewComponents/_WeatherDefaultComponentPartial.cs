using Linka.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Linka.WebUI.ViewComponents.DefaultViewComponents
{
    public class _WeatherDefaultComponentPartial : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public _WeatherDefaultComponentPartial(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SimpleWeatherViewModel();

            try
            {
                var client = _httpClientFactory.CreateClient();

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://open-weather13.p.rapidapi.com/city?city=istanbul&lang=EN")
                };

                request.Headers.Add("x-rapidapi-key", _configuration["RapidApi:WeatherKey"]);
                request.Headers.Add("x-rapidapi-host", "open-weather13.p.rapidapi.com");

                using var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();
                var weather = JsonConvert.DeserializeObject<WeatherViewModel.Rootobject>(body);

                model.City = "Istanbul";       // hep Istanbul görünsün
                model.Description = "Sunny";   // hep Sunny görünsün
                model.BackgroundImage = "/images/weather/sunny.png"; // hep aynı foto
                model.Temperature = weather.main.temp; // sadece dereceyi API'den al
            }
            catch
            {
                model.City = "Istanbul";
                model.Description = "Sunny";
                model.BackgroundImage = "/images/weather/sunny.png";
                model.Temperature = 0;
            }

            return View(model);
        }
    }
}
