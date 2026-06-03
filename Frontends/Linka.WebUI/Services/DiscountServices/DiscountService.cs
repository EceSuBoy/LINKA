using Linka.DtoLayer.DiscountDtos;
using System.Net;
using System.Net.Http.Json;

namespace Linka.WebUI.Services.DiscountServices
{
    public class DiscountService
        : IDiscountService
    {
        private readonly HttpClient
            _httpClient;

        public DiscountService(
            HttpClient httpClient)
        {
            _httpClient =
                httpClient;
        }

        public async Task<
            List<ResultDiscountCouponDto>>
            GetAllDiscountCouponsAsync()
        {
            var responseMessage =
                await _httpClient
                    .GetAsync(
                        "discount");

            await EnsureSuccessAsync(
                responseMessage,
                "Coupons could not be loaded.");

            return await responseMessage
                    .Content
                    .ReadFromJsonAsync<
                        List<ResultDiscountCouponDto>>()
                ?? new List<ResultDiscountCouponDto>();
        }

        public async Task<
            GetByIdDiscountCouponDto?>
            GetByIdDiscountCouponAsync(
                int id)
        {
            var responseMessage =
                await _httpClient
                    .GetAsync(
                        $"discount/{id}");

            if (responseMessage.StatusCode ==
                HttpStatusCode.NotFound)
            {
                return null;
            }

            await EnsureSuccessAsync(
                responseMessage,
                "Coupon could not be loaded.");

            return await responseMessage
                .Content
                .ReadFromJsonAsync<
                    GetByIdDiscountCouponDto>();
        }

        public async Task
            CreateDiscountCouponAsync(
                CreateDiscountCouponDto dto)
        {
            var responseMessage =
                await _httpClient
                    .PostAsJsonAsync(
                        "discount",
                        dto);

            await EnsureSuccessAsync(
                responseMessage,
                "Coupon could not be created.");
        }

        public async Task
            UpdateDiscountCouponAsync(
                UpdateDiscountCouponDto dto)
        {
            var responseMessage =
                await _httpClient
                    .PutAsJsonAsync(
                        "discount",
                        dto);

            await EnsureSuccessAsync(
                responseMessage,
                "Coupon could not be updated.");
        }

        public async Task
            DeleteDiscountCouponAsync(
                int id)
        {
            var responseMessage =
                await _httpClient
                    .DeleteAsync(
                        $"discount/{id}");

            await EnsureSuccessAsync(
                responseMessage,
                "Coupon could not be deleted.");
        }

        public async Task<int>
            GetDiscountCouponCountRate(
                string code)
        {
            var normalizedCode =
                Uri.EscapeDataString(
                    code.Trim()
                        .ToUpperInvariant());

            /*
             * Önceden doğrudan localhost:7071 kullanılıyordu.
             * Artık Program.cs içindeki BaseAddress üzerinden
             * Ocelot Gateway çağrısı yapılır.
             */
            var responseMessage =
                await _httpClient
                    .GetAsync(
                        "discount/" +
                        "GetDiscountCouponCountRate" +
                        $"?code={normalizedCode}");

            if (!responseMessage
                    .IsSuccessStatusCode)
            {
                return 0;
            }

            return await responseMessage
                    .Content
                    .ReadFromJsonAsync<int>();
        }

        private static async Task
            EnsureSuccessAsync(
                HttpResponseMessage responseMessage,
                string message)
        {
            if (responseMessage
                    .IsSuccessStatusCode)
            {
                return;
            }

            var content =
                await responseMessage
                    .Content
                    .ReadAsStringAsync();

            throw new Exception(
                $"{message} " +
                $"{responseMessage.StatusCode} - " +
                $"{content}");
        }
    }
}