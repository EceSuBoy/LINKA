using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListPaginationComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
             return View();
        }
    }
}
