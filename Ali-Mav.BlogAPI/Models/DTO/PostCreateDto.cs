using System.ComponentModel.DataAnnotations;

namespace Ali_Mav.BlogAPI.Models.DTO
{
    public class PostCreateDto
    {
        public int UserId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
