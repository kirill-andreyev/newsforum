using System.Security.Authentication;
using AutoMapper;
using BusinessLogic.Models;
using BusinessLogic.Services.Interfaces.Services;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using NewsForumApi.Database.Repositories;

namespace BusinessLogic.Services.Implementations.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentPL> GetComment(int id)
        {
            return _mapper.Map<CommentPL>(await _context.Comments.FirstOrDefaultAsync(x => x.Id == id));
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
            var dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == commentPL.UserId);
            var comment = _mapper.Map<Comment>(commentPL);
            comment.User = dbUser;
            var entity = await _context.Comments.AddAsync(comment);
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
            return _context.Comments.Include(x => x.User).Where(x => x.ArticleId == id).Select(_mapper.Map<CommentPL>).ToList();
        }
    }
}
