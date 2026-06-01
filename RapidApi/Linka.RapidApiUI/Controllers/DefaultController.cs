using Linka.RapidApiUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Linka.RapidApiUI.Controllers
{
    public class DefaultController : Controller
    {
        public async Task<IActionResult> WeatherDetail ()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://open-weather13.p.rapidapi.com/city?city=new%20york&lang=EN"),
                Headers =
    {
        { "x-rapidapi-key", "0c7b05080fmsh8249ed924f337d9p10e459jsn49ac3df6fbdb" },
        { "x-rapidapi-host", "open-weather13.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var weather = JsonConvert.DeserializeObject<WeatherViewModel.Rootobject>(body);

                ViewBag.temp = weather.main.temp;
                return View();
            }
        }
    }
}
