using System.ComponentModel.DataAnnotations;

namespace Ali_Mav.BlogAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
