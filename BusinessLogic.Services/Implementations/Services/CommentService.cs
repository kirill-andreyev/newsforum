using System.Security.Authentication;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using Mapping;
using Microsoft.EntityFrameworkCore;
using NewsForumApi.Database.Repositories;

namespace BusinessLogic.Services.Implementations.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommentPL> GetComment(int id)
        {
            return CommentMapper.MapToPL(await _context.Comments.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task DeleteComment(int id)
        {
            var dbComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (dbComment == null)
            {
                throw new Exception("Comment not found");
            }
            _context.Comments.Remove(dbComment);
            _context.SaveChanges();
        }

        public async Task<int> CreateComment(CommentPL commentPL)
        {
            var entity = await _context.Comments.AddAsync(CommentMapper.MapToDAL(commentPL));
            await _context.SaveChangesAsync();
            return entity.Entity.Id;
        }

        public async Task<int> UpdateComment(CommentPL commentPL)
        {
            var dbComment = _context.Comments.FirstOrDefault(x => x.Id == commentPL.ID);

            if (dbComment == null)
            {
                throw new Exception("Comment not found");
            }

            if (dbComment.UserId != commentPL.UserId)
            {
                throw new AuthenticationException("You can't delete that comment");
            }

            dbComment.Text = commentPL.Text;
            await _context.SaveChangesAsync();
            return dbComment.Id;
        }

        public async Task<IList<CommentPL>> GetCommentsList(int id)
        {
            return _context.Comments.Include(x => x.User).Where(x => x.ArticleId == id).Select(CommentMapper.MapToPL).ToList();
        }
    }
}
