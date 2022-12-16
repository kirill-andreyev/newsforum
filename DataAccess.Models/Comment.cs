using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Comment
        {
            public int Id { get; set; }
            public string? Text { get; set; }
            public int ArticleId { get; set; }
            public Article? Article { get; set; }
            public int UserId { get; set; }
            public User? User { get; set; }
        }
}
