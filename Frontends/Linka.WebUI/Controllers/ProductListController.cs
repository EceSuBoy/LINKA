using Linka.DtoLayer.CommentDtos;
using Linka.WebUI.Services.CommentServices;
using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.Controllers
{
    public class ProductListController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public ProductListController(
            ICommentService commentService,
            IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        public IActionResult Index(
    string? id,
    decimal? minPrice,
    decimal? maxPrice,
    string? sort = "default",
    int page = 1,
    int pageSize = 10)
        {
            ViewBag.directory1 = "Home Page";
            ViewBag.directory2 = "Products";
            ViewBag.directory3 = "Product List";

            if (page < 1)
            {
                page = 1;
            }

            if (pageSize != 10 &&
                pageSize != 20 &&
                pageSize != 30)
            {
                pageSize = 10;
            }

            var allowedSortValues =
                new[]
                {
            "default",
            "popularity",
            "best-rating",
            "price-asc",
            "price-desc"
                };

            if (string.IsNullOrWhiteSpace(sort) ||
                !allowedSortValues.Contains(
                    sort.ToLowerInvariant()))
            {
                sort = "default";
            }

            ViewBag.i =
                id;

            ViewBag.MinPrice =
                minPrice;

            ViewBag.MaxPrice =
                maxPrice;

            ViewBag.Sort =
                sort;

            ViewBag.CurrentPage =
                page;

            ViewBag.PageSize =
                pageSize;

            return View();
        }

        public async Task<IActionResult> ProductDetail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(
                    "Product ID cannot be empty.");
            }

            ViewBag.directory1 =
                "Home Page";

            ViewBag.directory2 =
                "Product List";

            ViewBag.directory3 =
                "Product Details";

            ViewBag.x =
                id;

            var statistic =
                await _commentService
                    .GetProductCommentStatisticsAsync(id);

            ViewBag.ReviewCount =
                statistic.CommentCount;

            ViewBag.AverageRating =
                statistic.AverageRating;

            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(
    string id,
    CreateCommentDto createCommentDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Product ID cannot be empty.");
            }

            if (createCommentDto.Rating < 1 ||
                createCommentDto.Rating > 5)
            {
                ModelState.AddModelError(
                    nameof(createCommentDto.Rating),
                    "Please select a rating between 1 and 5.");

                return RedirectToAction(
                    "ProductDetail",
                    "ProductList",
                    new { id });
            }

            var user =
                await _userService.GetUserInfo();

            /*
             * Kimlik bilgilerini formdan almıyoruz.
             * IdentityServer üzerinden giriş yapan kullanıcıdan alıyoruz.
             */
            createCommentDto.UserId = user.Id;

            createCommentDto.NameSurname =
                $"{user.Name} {user.Surname}".Trim();

            createCommentDto.Email = user.Email;

            /*
             * Ürün ID değeri route üzerinden garanti altına alınıyor.
             */
            createCommentDto.ProductId = id;

            createCommentDto.ImageUrl =
                "/images/userprofileavatar/avatarr.png";

            createCommentDto.CreatedDate =
                DateTime.Now;

            createCommentDto.Status =
                true;

            await _commentService
                .CreateCommentAsync(createCommentDto);

            return RedirectToAction(
                "ProductDetail",
                "ProductList",
                new { id });
        }
    }
}