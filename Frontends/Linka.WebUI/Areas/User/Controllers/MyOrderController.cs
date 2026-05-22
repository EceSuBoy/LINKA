using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.Controllers
{
    public class MyOrderController : Controller
    {
        [Area("User")]
        public IActionResult MyOrderList()
        {
            return View();
        }
    }
}
