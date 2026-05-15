using Linka.Comment.Context;
using Linka.Comment.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            _context.UserComments.Update(userComment);
            _context.SaveChanges();
            return Ok("Comment updated successfully.");
        }
    }
}
