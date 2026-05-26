
namespace Linka.WebUI.Services.StatisticServices.DiscountStatisticServices
{
    public class DiscountStatisticServices : IDiscountStatisticServices
    {
        private readonly HttpClient _httpClient;

        public DiscountStatisticServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> GetDiscountCouponCount()
        {
            var responseMessage = await _httpClient.GetAsync("discount/GetDiscountCouponCount");
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }
    }
}
