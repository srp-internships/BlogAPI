using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Numerics;

namespace Ali_Mav.BlogAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public List<Post> Posts { get; set; } = new List<Post>();

    }
}
