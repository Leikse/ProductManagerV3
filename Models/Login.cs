using System.ComponentModel.DataAnnotations;

namespace ProductManager.Models
{
    public class Login
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        public Login(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
