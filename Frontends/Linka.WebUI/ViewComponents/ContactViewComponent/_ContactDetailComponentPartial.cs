using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ContactViewComponent
{
    public class _ContactDetailComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
