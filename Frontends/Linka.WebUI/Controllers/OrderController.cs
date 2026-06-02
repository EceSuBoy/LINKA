using Linka.DtoLayer.OrderDtos.OrderAddressDtos;
using Linka.WebUI.Services.BasketServices;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.OrderServices.OrderAddressServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderAddressServices
            _orderAddressServices;

        private readonly IUserService
            _userService;

        private readonly IBasketService
            _basketService;

        public OrderController(
            IOrderAddressServices orderAddressServices,
            IUserService userService,
            IBasketService basketService)
        {
            _orderAddressServices =
                orderAddressServices;

            _userService =
                userService;

            _basketService =
                basketService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
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

            SetBreadcrumbs();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            CreateOrderAddressDto createOrderAddressDto,
            string? paymentMethod)
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

            /*
             * UserId değerini formdan istemiyoruz.
             * Giriş yapan kullanıcıdan otomatik alıyoruz.
             */
            var user =
                await _userService
                    .GetUserInfo();

            createOrderAddressDto.UserId =
                user.Id;

            /*
             * Description alanını ayrıca kullanıcıya
             * doldurtmak yerine adres satırlarından oluşturuyoruz.
             */
            createOrderAddressDto.Description =
                string.Join(
                    " ",
                    new[]
                    {
                        createOrderAddressDto.Detail1,
                        createOrderAddressDto.Detail2
                    }
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x)));

            /*
             * Model binding sırasında UserId ve Description
             * formdan gelmediği için validation hatası oluşmuş
             * olabilir. Değerleri yukarıda biz doldurduk.
             */
            ModelState.Remove(
                nameof(
                    CreateOrderAddressDto.UserId));

            ModelState.Remove(
                nameof(
                    CreateOrderAddressDto.Description));

            if (paymentMethod != "CreditCard")
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Please select Credit Card / Debit Card.");
            }

            if (!ModelState.IsValid)
            {
                SetBreadcrumbs();

                return View(
                    createOrderAddressDto);
            }

            await _orderAddressServices
                .CreateOrderAddressAsync(
                    createOrderAddressDto);

            return RedirectToAction(
                "Index",
                "Payment");
        }

        private void SetBreadcrumbs()
        {
            ViewBag.directory1 =
                "Home Page";

            ViewBag.directory2 =
                "Orders";

            ViewBag.directory3 =
                "Checkout";
        }
    }
}