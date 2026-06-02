using Linka.DtoLayer.OrderDtos.OrderOrderingDtos;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.OrderServices.OrderOrderingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class MyOrderController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderOrderingServices _orderOrderingService;

        public MyOrderController(
            IOrderOrderingServices orderOrderingService,
            IUserService userService)
        {
            _orderOrderingService =
                orderOrderingService;

            _userService =
                userService;
        }

        /*
         * Eski linkler bozulmasın.
         * Eski MyOrderList adresi aktif siparişlere gider.
         */
        [HttpGet]
        public IActionResult MyOrderList()
        {
            return RedirectToAction(
                nameof(ActiveOrders));
        }

        /*
         * Yalnızca devam eden siparişler:
         * Paid, Preparing, Shipped
         */
        [HttpGet]
        public async Task<IActionResult> ActiveOrders()
        {
            var values =
                await GetCurrentUserOrders();

            var activeOrders =
                values
                    .Where(x =>
                    {
                        var status =
                            NormalizeStatus(
                                x.OrderStatus);

                        return status == "paid" ||
                               status == "preparing" ||
                               status == "shipped";
                    })
                    .OrderByDescending(x =>
                        x.OrderDate)
                    .ToList();

            SetListPageViewBags(
                pageTitle:
                    "My Active Orders",

                pageSubtitle:
                    "Track your current orders and follow their delivery progress.",

                emptyMessage:
                    "You do not have any active orders.",

                selectedSection:
                    "active");

            return View(
                "MyOrderList",
                activeOrders);
        }

        /*
         * Yalnızca tamamlanan veya iptal edilen siparişler:
         * Delivered, Cancelled
         */
        [HttpGet]
        public async Task<IActionResult> OrderHistory()
        {
            var values =
                await GetCurrentUserOrders();

            var historyOrders =
                values
                    .Where(x =>
                    {
                        var status =
                            NormalizeStatus(
                                x.OrderStatus);

                        return status == "delivered" ||
                               status == "cancelled";
                    })
                    .OrderByDescending(x =>
                        x.OrderDate)
                    .ToList();

            SetListPageViewBags(
                pageTitle:
                    "Order History",

                pageSubtitle:
                    "Review your delivered and cancelled LINKA orders.",

                emptyMessage:
                    "Your order history is currently empty.",

                selectedSection:
                    "history");

            return View(
                "MyOrderList",
                historyOrders);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail(
            int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    "Order ID must be greater than zero.");
            }

            var user =
                await _userService
                    .GetUserInfo();

            var order =
                await _orderOrderingService
                    .GetOrderingDetailAsync(
                        id,
                        user.Id);

            if (order == null)
            {
                return NotFound(
                    "Order could not be found.");
            }

            return View(order);
        }

        private async Task<
            List<ResultOrderingByUserIdDto>>
            GetCurrentUserOrders()
        {
            var user =
                await _userService
                    .GetUserInfo();

            var values =
                await _orderOrderingService
                    .GetOrderingByUserId(
                        user.Id);

            return values ??
                new List<ResultOrderingByUserIdDto>();
        }

        /*
         * Başında veya sonunda boşluk varsa temizler.
         * Büyük-küçük harf farkını kaldırır.
         *
         * Örnek:
         * " Delivered "
         *      ↓
         * "delivered"
         */
        private static string NormalizeStatus(
            string? orderStatus)
        {
            return string.IsNullOrWhiteSpace(
                    orderStatus)
                ? "paid"
                : orderStatus
                    .Trim()
                    .ToLowerInvariant();
        }

        private void SetListPageViewBags(
            string pageTitle,
            string pageSubtitle,
            string emptyMessage,
            string selectedSection)
        {
            ViewBag.PageTitle =
                pageTitle;

            ViewBag.PageSubtitle =
                pageSubtitle;

            ViewBag.EmptyMessage =
                emptyMessage;

            ViewBag.SelectedSection =
                selectedSection;
        }
    }
}