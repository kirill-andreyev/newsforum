using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using DataAccess.Models;

namespace Mapping
{
    public class CommentMapper
    {
        public static CommentPL MapToPL(Comment comment)
        {
            return new CommentPL
            {
                UserName = comment.User.Name,
                ArticleId = comment.ArticleId,
                ID = comment.Id,
                Text = comment.Text
            };
        }

        public static Comment MapToDAL(CommentPL comment)
        {
            return new Comment
            {
                UserId = comment.UserId,
                ArticleId = comment.ArticleId,
                Text = comment.Text
            };
        }
    }
}
