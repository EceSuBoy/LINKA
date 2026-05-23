using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.User.ViewComponents.UserLayoutViewComponent
{
    public class _UserLayoutHeadComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
