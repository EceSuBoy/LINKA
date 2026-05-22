using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
