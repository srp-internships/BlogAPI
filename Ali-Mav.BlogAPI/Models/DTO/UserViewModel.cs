using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net;

namespace Ali_Mav.BlogAPI.Models.DTO
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }
    }
}
