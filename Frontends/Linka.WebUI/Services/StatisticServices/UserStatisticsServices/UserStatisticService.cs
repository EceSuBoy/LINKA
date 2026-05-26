
using Linka.DtoLayer.IdentityDtos.UserDtos;
using Newtonsoft.Json;

namespace Linka.WebUI.Services.StatisticServices.UserStatisticsServices
{
    public class UserStatisticService : IUserStatisticService
    {
        private readonly HttpClient _httpClient;

        public UserStatisticService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetUserCount()
        {
            var responseMessage = await _httpClient.GetAsync("http://localhost:5001/Api/Statistics");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<int>(jsonData);
            return values;
        }
    }
}
