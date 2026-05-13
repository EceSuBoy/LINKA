using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CarouselDefaultComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
