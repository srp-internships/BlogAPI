using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ali_Mav.BlogAPI.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
