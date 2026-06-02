using Linka.WebUI.Models;
using Linka.WebUI.Services.BasketServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.OrderViewComponents
{
    public class _OrderSummaryComponentPartial
        : ViewComponent
    {
        private readonly IBasketService _basketService;

        public _OrderSummaryComponentPartial(
            IBasketService basketService)
        {
            _basketService =
                basketService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var basket =
                await _basketService
                    .GetBasket();

            basket.BasketItems ??=
                new List<Linka.DtoLayer.BasketDtos.BasketItemDto>();

            var subTotal =
                Math.Round(
                    basket.TotalPrice,
                    2);

            var discountRate =
                basket.DiscountRate ?? 0;

            var discountAmount =
                Math.Round(
                    subTotal *
                    discountRate /
                    100m,
                    2);

            var discountedSubTotal =
                Math.Round(
                    subTotal -
                    discountAmount,
                    2);

            const decimal vatRate =
                10m;

            var vatAmount =
                Math.Round(
                    discountedSubTotal *
                    vatRate /
                    100m,
                    2);

            /*
             * 1.500 ₺ ve üzerindeki sepetlerde ücretsiz kargo.
             */
            const decimal freeShippingThreshold =
                1500m;

            var shippingFee =
                !basket.BasketItems.Any()
                    ? 0m
                    : discountedSubTotal >=
                      freeShippingThreshold
                        ? 0m
                        : 50m;

            var grandTotal =
                Math.Round(
                    discountedSubTotal +
                    vatAmount +
                    shippingFee,
                    2);

            var model =
                new OrderSummaryViewModel
                {
                    BasketItems =
                        basket.BasketItems,

                    DiscountCode =
                        basket.DiscountCode,

                    DiscountRate =
                        discountRate,

                    SubTotal =
                        subTotal,

                    DiscountAmount =
                        discountAmount,

                    DiscountedSubTotal =
                        discountedSubTotal,

                    VatRate =
                        vatRate,

                    VatAmount =
                        vatAmount,

                    ShippingFee =
                        shippingFee,

                    GrandTotal =
                        grandTotal
                };

            return View(model);
        }
    }
}