using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using DataAccess.Models;

namespace Mapping
{
    public class ArticleMapper
    {
        public static ArticlePL MapToPL(Article article)
        {
            return new ArticlePL
            {
                CreatedDate = article.CreatedTime,
                Description = article.Description,
                Title = article.Title,
                ImageName = article.ImageName,
                ID = article.Id,
            };
        }

        public static Article MapToDAL(ArticlePL article)
        {
            return new Article
            {
                CreatedTime = DateTime.UtcNow,
                Description = article.Description,
                Title = article.Title,
                ImageName = article.ImageName
            };
        }
    }

}
