using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.UILayoutViewComponents
{
    public class _TopbarUILayoutComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
