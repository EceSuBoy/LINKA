using Linka.DtoLayer.CommentDtos;
using Linka.WebUI.Services.CommentServices;
using Microsoft.AspNetCore.Mvc;

namespace Linka.WebUI.ViewComponents.ProductDetailViewComponents
{
    public class _ProductDetailReviewComponentPartial : ViewComponent
    {
        private readonly ICommentService _commentService;

        public _ProductDetailReviewComponentPartial(
            ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            /*
             * Partial view içindeki yorum ekleme formuna
             * hangi ürünün açık olduğunu iletiyoruz.
             */
            ViewBag.pid = id;

            var values =
                await _commentService.CommentListByProductId(id);

            return View(values ?? new List<ResultCommentDto>());
        }
    }
}