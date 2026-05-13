using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ShoppingCartViewComponents
{
    public class _ShoppingCartProductListComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
