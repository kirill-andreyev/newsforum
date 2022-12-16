using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllComments(int id)
        {
            return Ok(await _commentService.GetCommentsList(id));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            return Ok(await _commentService.GetComment(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(int ArticleId, string Title, int UserId)
        {
            var comment = new CommentPL { ArticleId = ArticleId, Text = Title, UserId = UserId};
            return Ok(await _commentService.CreateComment(comment));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(int ArticleId, string Title, int UserId, int CommentId)
        {
            var comment = new CommentPL { ArticleId = ArticleId, Text = Title, UserId = UserId, ID = CommentId};
            return Ok(await _commentService.UpdateComment(comment));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteComment(id);
            return Ok();
        }
    }
}
