namespace BusinessLogic.Models
{
    public class CommentPL
    {
        public int ID { get; set; }
        public int ArticleId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}
