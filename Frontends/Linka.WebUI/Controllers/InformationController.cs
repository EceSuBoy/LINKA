using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class InformationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
