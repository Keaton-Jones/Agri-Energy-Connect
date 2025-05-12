using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class User
    {
        [Key]
        public int farmerId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Surname")]
        public string Surname { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        [StringLength(10, ErrorMessage = "Phone number must be 10 digits long.", MinimumLength = 10)]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        [PasswordPropertyText]
        public string Password { get; set; }

        public string role { get; set; }
    }
}