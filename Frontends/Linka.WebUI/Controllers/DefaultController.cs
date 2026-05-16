using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
