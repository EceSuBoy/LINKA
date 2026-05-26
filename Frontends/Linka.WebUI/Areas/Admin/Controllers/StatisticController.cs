using Linka.WebUI.Services.CommentServices;
using Linka.WebUI.Services.StatisticServices.CatalogStatisticServices;
using Linka.WebUI.Services.StatisticServices.DiscountStatisticServices;
using Linka.WebUI.Services.StatisticServices.MessageStatisticServices;
using Linka.WebUI.Services.StatisticServices.UserStatisticsServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")] 
    public class StatisticController : Controller
    {
        private readonly ICatalogStatisticService _catalogStatisticService;
        private readonly IUserStatisticService _userstatisticService;
        private readonly ICommentService _commentService;
        private readonly IDiscountStatisticServices _discountStatisticServices;
        private readonly IMessageStatisticService _messageStatisticService;

        public StatisticController(ICatalogStatisticService catalogStatisticService, IUserStatisticService userstatisticService, ICommentService commentService, IDiscountStatisticServices discountStatisticServices, IMessageStatisticService messageStatisticService)
        {
            _catalogStatisticService = catalogStatisticService;
            _userstatisticService = userstatisticService;
            _commentService = commentService;
            _discountStatisticServices = discountStatisticServices;
            _messageStatisticService = messageStatisticService;
        }

        public async Task<IActionResult> Index()
        {
            var getBrandCount = await _catalogStatisticService.GetBrandCount();
            var getProductCount = await _catalogStatisticService.GetProductCount();
            var getCategoryCount = await _catalogStatisticService.GetCategoryCount();
            var getProductAvgPrice = await _catalogStatisticService.GetProductAvgPrice();
            var getMinPriceProductName = await _catalogStatisticService.GetMinPriceProductName();
            var getMaxPriceProductName = await _catalogStatisticService.GetMaxPriceProductName();

            var getUserCount = await _userstatisticService.GetUserCount();

            var getTotalCommentCount = await _commentService.GetTotalCommentCount();
            var getActiveCommentCount = await _commentService.GetActiveCommentCount();
            var getPassiveCommentCount = await _commentService.GetPassiveCommentCount();

            var getDiscountCouponCount = await _discountStatisticServices.GetDiscountCouponCount();

            var getTotalMessageCount = await _messageStatisticService.GetTotalMessageCount();



            ViewBag.getBrandCount = getBrandCount;
            ViewBag.getProductCount = getProductCount;
            ViewBag.getCategoryCount = getCategoryCount;
            ViewBag.getProductAvgPrice = getProductAvgPrice;
            ViewBag.getMinPriceProductName = getMinPriceProductName;
            ViewBag.getMaxPriceProductName = getMaxPriceProductName;

            ViewBag.getUserCount = getUserCount;

            ViewBag.getTotalCommentCount = getTotalCommentCount;
            ViewBag.getActiveCommentCount = getActiveCommentCount;
            ViewBag.getPassiveCommentCount = getPassiveCommentCount;

            ViewBag.getDiscountCouponCount = getDiscountCouponCount;

            ViewBag.getTotalMessageCount = getTotalMessageCount;

            return View();
        }
    }
}
