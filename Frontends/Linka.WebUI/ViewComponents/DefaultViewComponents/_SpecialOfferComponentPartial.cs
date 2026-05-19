using Linka.DtoLayer.CatalogDtos.SpecialOfferDtos;
using Linka.WebUI.Services.CatalogServices.SpecialOfferServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Linka.WebUI.ViewComponents.DefaultViewComponents
{
    public class _SpecialOfferComponentPartial : ViewComponent
    {
        private readonly ISpecialOfferService _specialOfferService;

        public _SpecialOfferComponentPartial(ISpecialOfferService specialOfferService)
        {
            _specialOfferService = specialOfferService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Special Offers";
            ViewBag.v3 = "Special Offers and Daily Discount List";
            ViewBag.v0 = "Special Offer Operations";

            var values = await _specialOfferService.GetAllSpecialOfferAsync();
            return View(values);

        }   
    }
}
