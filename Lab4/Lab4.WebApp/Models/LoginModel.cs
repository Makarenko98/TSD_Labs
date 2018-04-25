using System.ComponentModel.DataAnnotations;

namespace Lab4.WebApp.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is not specified")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Password is not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
