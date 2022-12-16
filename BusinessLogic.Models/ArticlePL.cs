using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Models
{
    public class ArticlePL
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
