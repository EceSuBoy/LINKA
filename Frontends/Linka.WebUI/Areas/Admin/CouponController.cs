using Linka.DtoLayer.DiscountDtos;
using Linka.WebUI.Models;
using Linka.WebUI.Services.DiscountServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Coupon")]
    public class CouponController
        : AdminControllerBase
    {
        private readonly IDiscountService
            _discountService;

        public CouponController(
            IDiscountService discountService)
        {
            _discountService =
                discountService;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult>
            Index(
                string? search,
                string? statusFilter = "All",
                int page = 1,
                int pageSize = 10)
        {
            ViewBag.v1 =
                "Home";

            ViewBag.v2 =
                "Coupons";

            ViewBag.v3 =
                "Coupon List";

            ViewBag.v0 =
                "Coupon Management";

            if (page < 1)
            {
                page =
                    1;
            }

            var allowedPageSizes =
                new[]
                {
                    10,
                    20,
                    50
                };

            if (!allowedPageSizes
                    .Contains(
                        pageSize))
            {
                pageSize =
                    10;
            }

            var allCoupons =
                await _discountService
                    .GetAllDiscountCouponsAsync();

            var today =
                DateTime.Today;

            var query =
                allCoupons
                    .AsEnumerable();

            if (!string.IsNullOrWhiteSpace(
                    search))
            {
                var searchValue =
                    search.Trim();

                query =
                    query.Where(x =>
                        x.Code.Contains(
                            searchValue,
                            StringComparison
                                .OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(
                    statusFilter) &&
                !string.Equals(
                    statusFilter,
                    "All",
                    StringComparison
                        .OrdinalIgnoreCase))
            {
                query =
                    statusFilter
                        .Trim()
                        .ToLowerInvariant()
                    switch
                    {
                        "active" =>
                            query.Where(x =>
                                x.IsActive &&
                                x.ValidDate.Date >=
                                today),

                        "inactive" =>
                            query.Where(x =>
                                !x.IsActive),

                        "expired" =>
                            query.Where(x =>
                                x.ValidDate.Date <
                                today),

                        _ =>
                            query
                    };
            }

            var filteredCoupons =
                query
                    .OrderByDescending(x =>
                        x.ValidDate)
                    .ThenBy(x =>
                        x.Code)
                    .ToList();

            var totalCount =
                filteredCoupons.Count;

            var totalPages =
                Math.Max(
                    1,
                    (int)Math.Ceiling(
                        totalCount /
                        (double)pageSize));

            if (page > totalPages)
            {
                page =
                    totalPages;
            }

            var items =
                filteredCoupons
                    .Skip(
                        (page - 1) *
                        pageSize)
                    .Take(
                        pageSize)
                    .ToList();

            var model =
                new CouponListViewModel
                {
                    Items =
                        items,

                    Search =
                        search?.Trim() ??
                        string.Empty,

                    StatusFilter =
                        string.IsNullOrWhiteSpace(
                            statusFilter)
                            ? "All"
                            : statusFilter,

                    Page =
                        page,

                    PageSize =
                        pageSize,

                    TotalCount =
                        totalCount,

                    TotalPages =
                        totalPages,

                    ActiveCount =
                        allCoupons.Count(x =>
                            x.IsActive &&
                            x.ValidDate.Date >=
                            today),

                    InactiveCount =
                        allCoupons.Count(x =>
                            !x.IsActive),

                    ExpiredCount =
                        allCoupons.Count(x =>
                            x.ValidDate.Date <
                            today)
                };

            return View(model);
        }

        [HttpGet]
        [Route("CreateCoupon")]
        public IActionResult
            CreateCoupon()
        {
            SetCreateViewBags();

            return View(
                new CreateDiscountCouponDto
                {
                    IsActive =
                        true,

                    ValidDate =
                        DateTime.Today
                            .AddMonths(1)
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("CreateCoupon")]
        public async Task<IActionResult>
            CreateCoupon(
                CreateDiscountCouponDto dto)
        {
            SetCreateViewBags();

            NormalizeCode(
                dto);

            ValidateCoupon(
                dto.Code,
                dto.Rate,
                dto.ValidDate);

            var coupons =
                await _discountService
                    .GetAllDiscountCouponsAsync();

            if (coupons.Any(x =>
                    string.Equals(
                        x.Code,
                        dto.Code,
                        StringComparison
                            .OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    nameof(dto.Code),
                    "This coupon code already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _discountService
                .CreateDiscountCouponAsync(
                    dto);

            TempData["CouponSuccess"] =
                "Coupon created successfully.";

            return RedirectToAction(
                nameof(Index));
        }

        [HttpGet]
        [Route("UpdateCoupon/{id:int}")]
        public async Task<IActionResult>
            UpdateCoupon(
                int id)
        {
            SetUpdateViewBags();

            var value =
                await _discountService
                    .GetByIdDiscountCouponAsync(
                        id);

            if (value == null)
            {
                return NotFound(
                    "Coupon could not be found.");
            }

            return View(
                new UpdateDiscountCouponDto
                {
                    CouponId =
                        value.CouponId,

                    Code =
                        value.Code,

                    Rate =
                        value.Rate,

                    IsActive =
                        value.IsActive,

                    ValidDate =
                        value.ValidDate
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UpdateCoupon/{id:int}")]
        public async Task<IActionResult>
            UpdateCoupon(
                int id,
                UpdateDiscountCouponDto dto)
        {
            SetUpdateViewBags();

            dto.CouponId =
                id;

            NormalizeCode(
                dto);

            ValidateCoupon(
                dto.Code,
                dto.Rate,
                dto.ValidDate,
                allowExpiredDate:
                    true);

            var coupons =
                await _discountService
                    .GetAllDiscountCouponsAsync();

            if (coupons.Any(x =>
                    x.CouponId !=
                    dto.CouponId &&
                    string.Equals(
                        x.Code,
                        dto.Code,
                        StringComparison
                            .OrdinalIgnoreCase)))
            {
                ModelState.AddModelError(
                    nameof(dto.Code),
                    "This coupon code already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            await _discountService
                .UpdateDiscountCouponAsync(
                    dto);

            TempData["CouponSuccess"] =
                "Coupon updated successfully.";

            return RedirectToAction(
                nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DeleteCoupon/{id:int}")]
        public async Task<IActionResult>
            DeleteCoupon(
                int id)
        {
            await _discountService
                .DeleteDiscountCouponAsync(
                    id);

            TempData["CouponSuccess"] =
                "Coupon deleted successfully.";

            return RedirectToAction(
                nameof(Index));
        }

        private void ValidateCoupon(
            string code,
            int rate,
            DateTime validDate,
            bool allowExpiredDate = false)
        {
            if (string.IsNullOrWhiteSpace(
                    code))
            {
                ModelState.AddModelError(
                    nameof(code),
                    "Coupon code cannot be empty.");
            }

            if (rate < 1 ||
                rate > 100)
            {
                ModelState.AddModelError(
                    nameof(rate),
                    "Discount rate must be between 1 and 100.");
            }

            if (!allowExpiredDate &&
                validDate.Date <
                DateTime.Today)
            {
                ModelState.AddModelError(
                    nameof(validDate),
                    "Expiration date cannot be in the past.");
            }
        }

        private static void NormalizeCode(
            CreateDiscountCouponDto dto)
        {
            dto.Code =
                dto.Code?
                    .Trim()
                    .ToUpperInvariant()
                ?? string.Empty;
        }

        private static void NormalizeCode(
            UpdateDiscountCouponDto dto)
        {
            dto.Code =
                dto.Code?
                    .Trim()
                    .ToUpperInvariant()
                ?? string.Empty;
        }

        private void SetCreateViewBags()
        {
            ViewBag.v1 =
                "Home";

            ViewBag.v2 =
                "Coupons";

            ViewBag.v3 =
                "Create Coupon";

            ViewBag.v0 =
                "Coupon Management";
        }

        private void SetUpdateViewBags()
        {
            ViewBag.v1 =
                "Home";

            ViewBag.v2 =
                "Coupons";

            ViewBag.v3 =
                "Update Coupon";

            ViewBag.v0 =
                "Coupon Management";
        }
    }
}