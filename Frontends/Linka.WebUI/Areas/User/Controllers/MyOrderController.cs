using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.OrderServices.OrderOrderingServices;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Linka.WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class MyOrderController : Controller
    {
        private readonly IUserService _userservice;
        private readonly IOrderOrderingServices _orderOrderingService;

        public MyOrderController(IOrderOrderingServices orderOrderingService, IUserService userservice)
        {
            _orderOrderingService = orderOrderingService;
            _userservice = userservice;
        }

        public async Task<IActionResult> MyOrderList()
        {
            var user = await _userservice.GetUserInfo();

            var values = await _orderOrderingService.GetOrderingByUserId(user.Id);
            return View(values);
        }
    }
}
