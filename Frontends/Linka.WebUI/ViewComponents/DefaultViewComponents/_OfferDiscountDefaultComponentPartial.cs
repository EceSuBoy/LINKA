using Linka.DtoLayer.CatalogDtos.OfferDiscountDtos;
using Linka.WebUI.Services.CatalogServices.OfferDiscountServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Linka.WebUI.ViewComponents.DefaultViewComponents
{
    public class _OfferDiscountDefaultComponentPartial :ViewComponent
    {
        private readonly IOfferDiscountService _offerDiscountService;

        public _OfferDiscountDefaultComponentPartial(IOfferDiscountService offerDiscountService)
        {
            _offerDiscountService = offerDiscountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _offerDiscountService.GetAllOfferDiscountAsync();
            return View(values);
        }
    }
}
