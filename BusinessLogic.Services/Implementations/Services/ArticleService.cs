using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NewsForumApi.Database.Repositories;

namespace BusinessLogic.Services.Implementations.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IMapper _mapper;
        public ArticleService(ApplicationDbContext context, IBlobStorageService blobStorageService, IMapper mapper)
        {
            _context = context;
            _blobStorageService = blobStorageService;
            _mapper = mapper;
        }
        public async Task<IList<ArticlePL>> GetArticleList()
        {
            return _context.Articles.Select(_mapper.Map<ArticlePL>).ToList();
        }

        public async Task<ArticlePL> GetArticle(int id)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(x => x.Id == id);

            if (article == null)
            {
                throw new Exception("Article doesn't exist");
            }

            var articlePL = _mapper.Map<ArticlePL>(article);
            articlePL.ImageUrl = await _blobStorageService.GetBlob(article.ImageName);

            return articlePL;
        }

        public async Task<int> CreateArticle(ArticlePL articlePL, IFormFile file)
        {
            articlePL.ImageName = file.FileName;
            articlePL.CreatedDate = DateTime.UtcNow;
            await _blobStorageService.UploadToBlobStorage(file);
            var entity = await _context.Articles.AddAsync(_mapper.Map<Article>(articlePL));
            await _context.SaveChangesAsync();
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

            dbArticle.CreatedTime = DateTime.UtcNow;
            dbArticle.Description = articlePL.Description;
            dbArticle.Title = articlePL.Title;
            dbArticle.ImageName = file.FileName;

            await _blobStorageService.UploadToBlobStorage(file);
            await _context.SaveChangesAsync();

            return dbArticle.Id;
        }

        public async Task DeleteArticle(int articleId)
        {
            var dbArticle = await _context.Articles.FirstOrDefaultAsync(x => x.Id == articleId);

            if (dbArticle == null)
            {
                throw new Exception("Article not found");
            }

            _context.Articles.Remove(dbArticle);
            await _context.SaveChangesAsync();
        }
    }
}
