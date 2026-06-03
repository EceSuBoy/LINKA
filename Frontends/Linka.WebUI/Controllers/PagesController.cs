using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class PagesController : Controller
    {
        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewBag.directory1 =
                "Home Page";

            ViewBag.directory2 =
                "Access Denied";

            ViewBag.directory3 =
                "Authorization Error";

            return View();
        }
    }
}