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

        public IActionResult Index(string id)
        {
            ViewBag.directory1 = "Home Page";
            ViewBag.directory2 = "Products";
            ViewBag.directory3 = "Product List";
            ViewBag.i = id;

            return View();
        }

        public IActionResult ProductDetail(string id)
        {
            ViewBag.directory1 = "Home Page";
            ViewBag.directory2 = "Product List";
            ViewBag.directory3 = "Product Details";

            // AddComment partial view'ine aktarılacak ürün ID değeri
            ViewBag.x = id;

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
                "/images/userprofileavatar/avatargirl.png";

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