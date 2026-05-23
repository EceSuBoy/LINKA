using Linka.DtoLayer.CommentDtos;
using Linka.DtoLayer.OrderDtos.OrderOrderingDtos;
using Newtonsoft.Json;

namespace Linka.WebUI.Services.OrderServices.OrderOrderingServices
{
    public class OrderOrderingServices : IOrderOrderingServices
    {
        private readonly HttpClient _httpClient;

        public OrderOrderingServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id)
        {
            var responseMessage = await _httpClient.GetAsync($"orderings/GetOrderingByUserId/{id}");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultOrderingByUserIdDto>>(jsonData);
            return values;
        }
    }
}
