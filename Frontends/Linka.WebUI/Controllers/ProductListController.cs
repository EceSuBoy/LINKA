using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductDetail()
        {
            return View();
        }
    }
}
