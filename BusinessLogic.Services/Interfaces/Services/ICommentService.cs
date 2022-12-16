using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace BusinessLogic.Services.Interfaces.Services
{
    public interface ICommentService
    {
        Task<CommentPL> GetComment(int id);
        Task DeleteComment(int id);
        Task<int> CreateComment (CommentPL commentPL);
        Task<int> UpdateComment (CommentPL commentPL);
        Task<IList<CommentPL>> GetCommentsList(int id);
    }
}
