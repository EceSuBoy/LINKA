using Linka.DtoLayer.BasketDtos;
using Linka.DtoLayer.CatalogDtos.ProductDtos;
using Linka.DtoLayer.OrderDtos.OrderOrderingDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.BasketServices;
using Linka.WebUI.Services.CatalogServices.ProductServices;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.OrderServices.OrderOrderingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Linka.WebUI.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IBasketService
    _basketService;

        private readonly IUserService
            _userService;

        private readonly IOrderOrderingServices
            _orderOrderingServices;

        private readonly IProductService
    _productService;

        public PaymentController(
            IBasketService basketService, IUserService userService, IOrderOrderingServices orderOrderingServices, IProductService productService)
        {
            _basketService =
                basketService;
            _userService = userService;
            _orderOrderingServices = orderOrderingServices;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var basket =
                await _basketService
                    .GetBasket();

            /*
             * URL doğrudan yazılarak boş sepetle
             * ödeme sayfasına girilmesini engelliyoruz.
             */
            if (basket.BasketItems == null ||
                !basket.BasketItems.Any())
            {
                TempData["CartError"] =
                    "Your cart is empty.";

                return RedirectToAction(
                    "Index",
                    "ShoppingCart");
            }

            SetBreadcrumbs();

            return View(
                new PaymentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            PaymentViewModel paymentViewModel)
        {
            var basket =
                await _basketService
                    .GetBasket();

            if (basket.BasketItems == null ||
                !basket.BasketItems.Any())
            {
                TempData["CartError"] =
                    "Your cart is empty.";

                return RedirectToAction(
                    "Index",
                    "ShoppingCart");
            }

            var normalizedCardNumber =
                Regex.Replace(
                    paymentViewModel.CardNumber ??
                    string.Empty,
                    @"\D",
                    string.Empty);

            if (normalizedCardNumber.Length != 16 ||
                !IsValidCardNumber(normalizedCardNumber))
            {
                ModelState.AddModelError(
                    nameof(
                        PaymentViewModel.CardNumber),
                    "Please enter a valid test card number.");
            }

            if (paymentViewModel.ExpirationMonth.HasValue &&
                paymentViewModel.ExpirationYear.HasValue)
            {
                var month =
                    paymentViewModel
                        .ExpirationMonth
                        .Value;

                var year =
                    paymentViewModel
                        .ExpirationYear
                        .Value;

                if (month < 1 ||
                    month > 12)
                {
                    ModelState.AddModelError(
                        nameof(
                            PaymentViewModel.ExpirationMonth),
                        "Please select a valid expiration month.");
                }
                else
                {
                    var lastDayOfMonth =
                        DateTime.DaysInMonth(
                            year,
                            month);

                    var expirationDate =
                        new DateTime(
                            year,
                            month,
                            lastDayOfMonth);

                    if (expirationDate <
                        DateTime.Today)
                    {
                        ModelState.AddModelError(
                            nameof(
                                PaymentViewModel.ExpirationYear),
                            "The selected card has expired.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                SetBreadcrumbs();

                return View(
                    paymentViewModel);
            }

            foreach (var item in basket.BasketItems)
            {
                var product =
                    await _productService
                        .GetByIdProductAsync(
                            item.ProductId);

                if (product == null)
                {
                    TempData["BasketError"] =
                        $"{item.ProductName} could not be found.";

                    return RedirectToAction(
                        "Index",
                        "ShoppingCart");
                }

                if (product.StockCount < item.Quantity)
                {
                    TempData["BasketError"] =
                        $"Insufficient stock for {item.ProductName}. Available stock: {product.StockCount}.";

                    return RedirectToAction(
                        "Index",
                        "ShoppingCart");
                }
            }

            var user =
                await _userService
                    .GetUserInfo();

            var grandTotal =
                CalculateGrandTotal(
                    basket);

            var createOrderingDto =
                new CreateOrderingWithDetailsDto
                {
                    UserId =
                        user.Id,

                    TotalPrice =
                        grandTotal,

                    OrderDetails =
                        basket.BasketItems
                            .Select(item =>
                                new CreateOrderingDetailItemDto
                                {
                                    ProductId =
                                        item.ProductId,

                                    ProductName =
                                        item.ProductName,

                                    ProductPrice =
                                        item.Price,

                                    ProductAmount =
                                        item.Quantity
                                })
                            .ToList()
                };

            var orderingId =
    await _orderOrderingServices
        .CreateOrderingWithDetailsAsync(
            createOrderingDto);

            foreach (var item in basket.BasketItems)
            {
                await _productService
                    .DecreaseProductStockAsync(
                        new DecreaseProductStockDto
                        {
                            ProductId =
                                item.ProductId,

                            Quantity =
                                item.Quantity
                        });
            }

            await _basketService
                .ClearBasket();

            var orderNumber =
                $"LNK-" +
                $"{DateTime.UtcNow:yyyyMMdd}-" +
                $"{orderingId:D6}";

            TempData["OrderNumber"] =
                orderNumber;

            return RedirectToAction(
                nameof(Success));
        }

        [HttpGet]
        public IActionResult Success()
        {
            if (TempData["OrderNumber"] == null)
            {
                return RedirectToAction(
                    "Index",
                    "Default");
            }

            ViewBag.OrderNumber =
                TempData["OrderNumber"];

            ViewBag.directory1 =
                "Home Page";

            ViewBag.directory2 =
                "Payment";

            ViewBag.directory3 =
                "Order Completed";

            return View();
        }

        private void SetBreadcrumbs()
        {
            ViewBag.directory1 =
                "Home Page";

            ViewBag.directory2 =
                "Checkout";

            ViewBag.directory3 =
                "Payment";
        }

        private static bool IsValidCardNumber(
            string cardNumber)
        {
            var sum =
                0;

            var shouldDouble =
                false;

            for (var index =
                    cardNumber.Length - 1;
                index >= 0;
                index--)
            {
                var digit =
                    cardNumber[index] -
                    '0';

                if (shouldDouble)
                {
                    digit *= 2;

                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum +=
                    digit;

                shouldDouble =
                    !shouldDouble;
            }

            return sum % 10 == 0;
        }

        private static decimal CalculateGrandTotal(
    BasketTotalDto basket)
        {
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

            const decimal freeShippingThreshold =
                1500m;

            var shippingFee =
                discountedSubTotal >=
                freeShippingThreshold
                    ? 0m
                    : 50m;

            return Math.Round(
                discountedSubTotal +
                vatAmount +
                shippingFee,
                2);
        }
    }
}