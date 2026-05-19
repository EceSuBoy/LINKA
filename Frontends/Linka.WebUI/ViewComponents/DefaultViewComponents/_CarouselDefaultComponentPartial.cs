using Linka.DtoLayer.CatalogDtos.FeatureSliderDtos;
using Linka.WebUI.Services.CatalogServices.SliderServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Linka.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CarouselDefaultComponentPartial: ViewComponent
    {
        private readonly IFeatureSliderService _featureSliderService;

        public _CarouselDefaultComponentPartial(IFeatureSliderService featureSliderService)
        {
            _featureSliderService = featureSliderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _featureSliderService.GetAllFeatureSliderAsync();
            return View(values);
        }
    }
}
