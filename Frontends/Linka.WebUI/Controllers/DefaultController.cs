using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            @ViewBag.directory1 = "Home Page";
            @ViewBag.directory3 = "Product List";
            return View();
        }
    }
}
