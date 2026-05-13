using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
