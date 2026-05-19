using Linka.DtoLayer.CatalogDtos.OfferDiscountDtos;
using Linka.WebUI.Services.CatalogServices.OfferDiscountServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/OfferDiscount")]
    public class OfferDiscountController : Controller
    {
        private readonly IOfferDiscountService _offerDiscountService;

        public OfferDiscountController(IOfferDiscountService offerDiscountService)
        {
            _offerDiscountService = offerDiscountService;
        }

        [Route("Index")]

        public async Task<IActionResult> Index()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Discount Offer";
            ViewBag.v3 = "Discount Offer List";
            ViewBag.v0 = "Discount Offer Operations";

            var values = await _offerDiscountService.GetAllOfferDiscountAsync();
            return View(values);
        }
        [HttpGet]
        [Route("CreateOfferDiscount")]
        public IActionResult CreateOfferDiscount()
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Discount Offer";
            ViewBag.v3 = "Add New Discount Offer";
            ViewBag.v0 = "Discount Offer Operations";
            return View();
        }

        [HttpPost]
        [Route("CreateOfferDiscount")]
        public async Task<IActionResult> CreateOfferDiscount(CreateOfferDiscountDto createOfferDiscountDto)
        {
            await _offerDiscountService.CreateOfferDiscountAsync(createOfferDiscountDto);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
        }
        [Route("DeleteOfferDiscount/{id}")]
        public async Task<IActionResult> DeleteOfferDiscount(string id)
        {
            await _offerDiscountService.DeleteOfferDiscountAsync(id);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
        }
        [Route("UpdateOfferDiscount/{id}")]
        public async Task<IActionResult> UpdateOfferDiscount(string id)
        {
            ViewBag.v1 = "Home";
            ViewBag.v2 = "Discount Offer";
            ViewBag.v3 = "Update Discount Offer";
            ViewBag.v0 = "Discount Offer Operations";

            var values = await _offerDiscountService.GetByIdOfferDiscountAsync(id);
            var updateCategoryDto = new UpdateOfferDiscountDto
            {
                OfferDiscountId = values.OfferDiscountId,
                Title = values.Title,
                SubTitle = values.SubTitle,
                ButtonTitle = values.ButtonTitle,
                ImageUrl = values.ImageUrl,
            };
            return View(updateCategoryDto);
        }
        [HttpPost]
        [Route("UpdateOfferDiscount/{id}")]
        public async Task<IActionResult> UpdateOfferDiscount(UpdateOfferDiscountDto updateOfferDiscountDto)
        {
            await _offerDiscountService.UpdateOfferDiscountAsync(updateOfferDiscountDto);
            return RedirectToAction("Index", "OfferDiscount", new { area = "Admin" });
        }
    }
}
