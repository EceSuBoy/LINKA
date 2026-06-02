using Linka.WebUI.Services.BasketServices;
using Linka.WebUI.Services.DiscountServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly IBasketService _basketService;

        public DiscountController(IDiscountService discountService, IBasketService basketService)
        {
            _discountService = discountService;
            _basketService = basketService;
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmDiscountCoupon()
        {                       
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDiscountCoupon(
    string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                TempData["CouponError"] =
                    "Please enter a coupon code.";

                return RedirectToAction(
                    "Index",
                    "ShoppingCart");
            }

            code =
                code.Trim()
                    .ToUpperInvariant();

            var discountRate =
                await _discountService
                    .GetDiscountCouponCountRate(code);

            /*
             * Kod bulunamadığında mevcut endpoint 0 döndürüyor.
             */
            if (discountRate <= 0)
            {
                TempData["CouponError"] =
                    "Coupon code is invalid or inactive.";

                return RedirectToAction(
                    "Index",
                    "ShoppingCart");
            }

            var basket =
                await _basketService
                    .GetBasket();

            basket.DiscountCode =
                code;

            basket.DiscountRate =
                discountRate;

            await _basketService
                .SaveBasket(basket);

            TempData["CouponSuccess"] =
                $"Coupon {code} has been applied successfully.";

            return RedirectToAction(
                "Index",
                "ShoppingCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDiscountCoupon()
        {
            var basket =
                await _basketService
                    .GetBasket();

            basket.DiscountCode =
                null;

            basket.DiscountRate =
                0;

            await _basketService
                .SaveBasket(basket);

            TempData["CouponSuccess"] =
                "Coupon has been removed.";

            return RedirectToAction(
                "Index",
                "ShoppingCart");
        }
    }
}
