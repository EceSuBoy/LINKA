using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
