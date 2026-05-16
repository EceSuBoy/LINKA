using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
