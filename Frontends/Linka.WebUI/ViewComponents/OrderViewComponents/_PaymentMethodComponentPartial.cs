using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.OrderViewComponents
{
    public class _PaymentMethodComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
