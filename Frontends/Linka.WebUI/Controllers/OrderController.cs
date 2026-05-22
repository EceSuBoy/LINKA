using Linka.DtoLayer.OrderDtos.OrderAddressDtos;
using Linka.WebUI.Services.Interfaces;
using Linka.WebUI.Services.OrderServices.OrderAddressServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Linka.WebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderAddressServices _orderAddressServices;
        private readonly IUserService _userService;

        public OrderController(IOrderAddressServices orderAddressServices, IUserService userService)
        {
            _orderAddressServices = orderAddressServices;
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.directory1 = "Home Page";
            ViewBag.directory2 = "Orders";
            ViewBag.directory3 = "Order Operations";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateOrderAddressDto createOrderAddressDto)
        {            
            var values = await _userService.GetUserInfo();
            createOrderAddressDto.UserId = values.Id;
            createOrderAddressDto.Description = "aa";

            await _orderAddressServices.CreateOrderAddressAsync(createOrderAddressDto);

            return RedirectToAction("Index", "Payment");
        }
    }
}
