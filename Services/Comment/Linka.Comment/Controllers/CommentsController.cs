using Linka.Comment.Context;
using Linka.Comment.Dtos;
using Linka.Comment.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Linka.Comment.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentContext _context;

        public CommentsController(CommentContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CommentList()
        {
            var values = _context.UserComments.ToList();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateComment(UserComment userComment)
        {
            _context.UserComments.Add(userComment);
            _context.SaveChanges();
            return Ok("Comment added successfully.");
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment = _context.UserComments.Find(id);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }
            _context.UserComments.Remove(comment);
            _context.SaveChanges();
            return Ok("Comment deleted successfully.");
        }
        [HttpGet("{id}")]
        public IActionResult GetComment(int id) {
            var comment = _context.UserComments.Find(id);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }
            return Ok(comment);
        }

        [HttpPut]
        public IActionResult UpdateComment(UserComment userComment)
        {
            var existingComment = _context.UserComments
                .Find(userComment.UserCommentId);

            if (existingComment == null)
            {
                return NotFound("Comment not found.");
            }

            existingComment.NameSurname = userComment.NameSurname;
            existingComment.ImageUrl = userComment.ImageUrl;
            existingComment.Email = userComment.Email;
            existingComment.CommentDetail = userComment.CommentDetail;
            existingComment.Rating = userComment.Rating;
            existingComment.CreatedDate = userComment.CreatedDate;
            existingComment.Status = userComment.Status;
            existingComment.ProductId = userComment.ProductId;

            if (!string.IsNullOrWhiteSpace(userComment.UserId))
            {
                existingComment.UserId = userComment.UserId;
            }

            _context.SaveChanges();

            return Ok("Comment updated successfully.");
        }
        [HttpGet("CommentListByProductId/{id}")]
        public IActionResult CommentListByProductId(string id)
        {
            var values = _context.UserComments
        .Where(x => x.ProductId == id)
        .ToList();

            return Ok(values);
        }

        [HttpGet("GetActiveCommentCount")]
        public IActionResult GetActiveCommentCount()
        {
            int value = _context.UserComments.Where(x=>x.Status== true).Count();
            return Ok(value);
        }

        [HttpGet("GetPassiveCommentCount")]
        public IActionResult GetPassiveCommentCount()
        {
            int value = _context.UserComments.Where(x => x.Status == false).Count();
            return Ok(value);
        }

        [HttpGet("GetTotalCommentCount")]
        public IActionResult GetTotalCommentCount()
        {
            int value = _context.UserComments.Count();
            return Ok(value);
        }

        [HttpGet("GetProductCommentStatistics/{productId}")]
        public async Task<IActionResult> GetProductCommentStatistics(
    string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                return BadRequest(
                    "Product ID cannot be empty.");
            }

            var activeComments =
                _context.UserComments
                    .Where(x =>
                        x.ProductId == productId &&
                        x.Status == true);

            var commentCount =
                await activeComments.CountAsync();

            double averageRating = 0;

            if (commentCount > 0)
            {
                averageRating =
                    await activeComments
                        .AverageAsync(x => x.Rating);
            }

            var value =
                new ProductCommentStatisticDto
                {
                    ProductId =
                        productId,

                    CommentCount =
                        commentCount,

                    AverageRating =
                        Math.Round(averageRating, 1)
                };

            return Ok(value);
        }

        [HttpGet("GetAllProductCommentStatistics")]
        public async Task<IActionResult>
    GetAllProductCommentStatistics()
        {
            var values =
                await _context.UserComments
                    .Where(x => x.Status == true)
                    .GroupBy(x => x.ProductId)
                    .Select(group =>
                        new ProductCommentStatisticDto
                        {
                            ProductId =
                                group.Key,

                            CommentCount =
                                group.Count(),

                            AverageRating =
                                group.Average(x =>
                                    (double)x.Rating)
                        })
                    .ToListAsync();

            foreach (var item in values)
            {
                item.AverageRating =
                    Math.Round(item.AverageRating, 1);
            }

            return Ok(values);
        }
    }
}
