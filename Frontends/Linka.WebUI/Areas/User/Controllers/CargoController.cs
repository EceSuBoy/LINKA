using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.Controllers
{
    public class CargoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
