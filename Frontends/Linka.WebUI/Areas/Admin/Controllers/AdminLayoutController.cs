using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminLayoutController : AdminControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
