using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services.Interfaces.Services
{
    public interface IArticleService
    {
        public Task<IList<ArticlePL>> GetArticleList();
        public Task<ArticlePL> GetArticle(int id);
        public Task<int> CreateArticle(ArticlePL articlePL, IFormFile file);
        public Task<int> UpdateArticle(ArticlePL articlePL, IFormFile file);
        public Task DeleteArticle(int articleId);
    }
}
