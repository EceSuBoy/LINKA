using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class UILayoutController : Controller
    {
        public IActionResult _UILayout()
        {
            return View();
        }
    }
}
