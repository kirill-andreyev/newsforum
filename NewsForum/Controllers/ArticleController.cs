using BusinessLogic.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Models;

namespace NewsForum.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IBlobStorageService _blobStorageService;

        public ArticleController(IArticleService articleService, IBlobStorageService blobStorageService)
        {
            _articleService = articleService;
            _blobStorageService = blobStorageService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            return Ok(await _articleService.GetArticleList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            return Ok(await _articleService.GetArticle(id));
        }

        [Authorize(Roles = Roles.adminRole)]
        [Authorize(Roles = Roles.moderatorRole)]
        [HttpPost]
        public async Task<IActionResult> CreateArticle(string Title, string Description, IFormFile file)
        {
            var article = new ArticlePL { Description = Description, Title = Title };
            
            return Ok(await _articleService.CreateArticle(article, file));
        }

        [Authorize(Roles = Roles.adminRole)]
        [Authorize(Roles = Roles.moderatorRole)]
        [HttpPut]
        public async Task<IActionResult> UpdateArticle(int id, string Title, string Description, IFormFile file)
        {
            var article = new ArticlePL { Title = Title, Description = Description };
           
            return Ok(await _articleService.UpdateArticle(article, file));
        }

        [Authorize(Roles = Roles.adminRole)]
        [Authorize(Roles = Roles.moderatorRole)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            await _articleService.DeleteArticle(id);
            return Ok();
        }
    }
}
