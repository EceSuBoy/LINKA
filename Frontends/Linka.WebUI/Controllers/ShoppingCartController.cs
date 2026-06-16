using Linka.DtoLayer.BasketDtos;
using Linka.WebUI.Services.BasketServices;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductService
            _productService;

        private readonly IBasketService
            _basketService;

        public ShoppingCartController(
            IProductService productService,
            IBasketService basketService)
        {
            _productService =
                productService;

            _basketService =
                basketService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.directory1 =
                "Home Page";

            ViewBag.directory2 =
                "Products";

            ViewBag.directory3 =
                "My Cart";

            var basket =
                await _basketService
                    .GetBasket();

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

            var tax =
                Math.Round(
                    discountedSubTotal *
                    vatRate /
                    100m,
                    2);

            var grandTotal =
                Math.Round(
                    discountedSubTotal +
                    tax,
                    2);

            ViewBag.code =
                basket.DiscountCode;

            ViewBag.discountRate =
                discountRate;

            ViewBag.total =
                subTotal;

            ViewBag.discountAmount =
                discountAmount;

            ViewBag.discountedSubTotal =
                discountedSubTotal;

            ViewBag.tax =
                tax;

            ViewBag.totalPriceWithTax =
                grandTotal;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddBasketItem(
            string id)
        {
            return await AddItemToBasket(
                id,
                1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            AddBasketItemWithQuantity(
                string id,
                int quantity)
        {
            return await AddItemToBasket(
                id,
                quantity);
        }

        private async Task<IActionResult> AddItemToBasket(
            string id,
            int quantity)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(
                    "Product ID cannot be empty.");
            }

            quantity =
                Math.Clamp(
                    quantity,
                    1,
                    99);

            var product =
                await _productService
                    .GetByIdProductAsync(id);

            if (product == null)
            {
                return NotFound(
                    "Product could not be found.");
            }

            var hasDiscount =
                product.DiscountRate > 0 &&
                product.DiscountRate <= 100;

            var salePrice =
                hasDiscount
                    ? product.ProductPrice -
                      (product.ProductPrice *
                       product.DiscountRate / 100m)
                    : product.ProductPrice;

            var item =
                new BasketItemDto
                {
                    ProductId =
                        product.ProductId,

                    ProductName =
                        product.ProductName,

                    ProductImageUrl =
                        product.ProductImageUrl,

                    Price =
                        salePrice,

                    Quantity =
                        quantity
                };

            await _basketService
                .AddBasketItem(item);

            if (product.StockCount <= 0)
            {
                TempData["BasketError"] =
                    "This product is currently out of stock.";

                return RedirectToAction(
                    "ProductDetail",
                    "ProductList",
                    new { id });
            }

            var basket =
                await _basketService
                    .GetBasket();

            var currentQuantity =
                basket.BasketItems
                    .FirstOrDefault(x =>
                        x.ProductId == id)
                    ?.Quantity
                ?? 0;

            if (currentQuantity + quantity >
                product.StockCount)
            {
                TempData["BasketError"] =
                    $"Only {product.StockCount} items are available in stock.";

                return RedirectToAction(
                    "ProductDetail",
                    "ProductList",
                    new { id });
            }

            return RedirectToAction(
                nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(
            string id)
        {
            await _basketService
                .RemoveBasketItem(id);

            return RedirectToAction(
                nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
    UpdateBasketItemQuantity(
        string id,
        int quantity)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(
                    "Product ID cannot be empty.");
            }

            if (quantity > 0)
            {
                var product =
                    await _productService
                        .GetByIdProductAsync(id);

                if (product == null)
                {
                    TempData["BasketError"] =
                        "Product could not be found.";

                    return RedirectToAction(
                        nameof(Index));
                }

                if (product.StockCount <= 0)
                {
                    TempData["BasketError"] =
                        "This product is currently out of stock.";

                    return RedirectToAction(
                        nameof(Index));
                }

                if (quantity > product.StockCount)
                {
                    TempData["BasketError"] =
                        $"Only {product.StockCount} items are available in stock.";

                    return RedirectToAction(
                        nameof(Index));
                }
            }

            await _basketService
                .UpdateBasketItemQuantity(
                    id,
                    quantity);

            return RedirectToAction(
                nameof(Index));
        }
    }
}