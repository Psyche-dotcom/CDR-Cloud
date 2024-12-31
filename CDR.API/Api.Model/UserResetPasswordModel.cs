using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CDR.API.Api.Model
{
    public class UserResetPasswordModel
    {
        public string Token { get; set; }
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(5, ErrorMessage = "CharacterLess")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("RePassword")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(5, ErrorMessage = "CharacterLess")]
        [Compare(nameof(Password), ErrorMessage = "PasswordNotMatch")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
