using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewsForumApi.Database.Repositories;

namespace BusinessLogic.Services.Implementations.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;
        public ArticleService(ApplicationDbContext context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }
        public async Task<IList<ArticlePL>> GetArticleList()
        {
            return _context.Articles.Select(ArticleMapper.MapToPL).ToList();
        }

        public async Task<ArticlePL> GetArticle(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(x => x.Id == id);

            if (article == null)
            {
                throw new Exception("Article doesn't exist");
            }

            var articlePL = ArticleMapper.MapToPL(article);
            articlePL.ImageUrl = await _blobStorageService.GetBlob(article.ImageName);

            return articlePL;
        }

        public async Task<int> CreateArticle(ArticlePL articlePL, IFormFile file)
        {
            articlePL.ImageName = file.FileName;
            articlePL.CreatedDate = DateTime.UtcNow;
            await _blobStorageService.UploadToBlobStorage(file);
            var entity = await _context.Articles.AddAsync(ArticleMapper.MapToDAL(articlePL));
            return entity.Entity.Id;
        }

        public async Task<int> UpdateArticle(ArticlePL articlePL, IFormFile file)
        {
            var dbArticle = await _context.Articles.FirstOrDefaultAsync(x => x.Id == articlePL.ID);
            if (dbArticle == null)
            {
                throw new Exception("Article doesn't exist");
            }

            await _blobStorageService.DeleteBlob(articlePL.ImageName);

            dbArticle.CreatedTime = DateTime.Now;
            dbArticle.Description = articlePL.Description;
            dbArticle.Title = articlePL.Title;
            dbArticle.ImageName = file.FileName;

            await _blobStorageService.UploadToBlobStorage(file);
            await _context.SaveChangesAsync();

            return dbArticle.Id;
        }

        public Task DeleteArticle(int articleId)
        {
            throw new NotImplementedException();
        }
    }
}
