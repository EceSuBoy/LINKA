using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.directory1 = "Home Page";
            ViewBag.directory3 = "Payment Page";

            return View();
        }
    }
}
