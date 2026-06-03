using Linka.DtoLayer.OrderDtos
    .OrderOrderingDtos;
using Linka.WebUI.Services.OrderServices
    .OrderOrderingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/Order")]
    public class OrderController : AdminControllerBase
    {
        private readonly IOrderOrderingServices
            _orderOrderingServices;

        public OrderController(
            IOrderOrderingServices orderOrderingServices)
        {
            _orderOrderingServices =
                orderOrderingServices;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.v1 =
                "Home";

            ViewBag.v2 =
                "Orders";

            ViewBag.v3 =
                "Order List";

            ViewBag.v0 =
                "Order Operations";

            var values =
                await _orderOrderingServices
                    .GetAllOrderingAsync();

            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UpdateStatus")]
        public async Task<IActionResult>
            UpdateStatus(
                UpdateOrderingStatusDto dto)
        {
            if (dto.OrderingId <= 0)
            {
                TempData["OrderError"] =
                    "Invalid order ID.";

                return RedirectToAction(
                    nameof(Index));
            }

            await _orderOrderingServices
                .UpdateOrderingStatusAsync(dto);

            TempData["OrderSuccess"] =
                "Order status updated successfully.";

            return RedirectToAction(
                nameof(Index));
        }
    }
}