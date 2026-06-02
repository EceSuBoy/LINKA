using Linka.DtoLayer.CommentDtos;
using Linka.WebUI.Services.CommentServices;
using Linka.WebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailReviewComponentPartial : ViewComponent
    {
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;

        public _ProductDetailReviewComponentPartial(
            ICommentService commentService,
            IUserService userService)
        {
            _commentService = commentService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            ViewBag.pid = id;

            ViewBag.CurrentUserId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var currentUser =
                    await _userService.GetUserInfo();

                ViewBag.CurrentUserId =
                    currentUser.Id;
            }

            var values =
                await _commentService
                    .CommentListByProductId(id);

            return View(
                values ?? new List<ResultCommentDto>());
        }
    }
}