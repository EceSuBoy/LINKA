using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.ViewComponents.UserLayoutViewComponent
{
    public class _UserLayoutNavbarComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
