using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class UserRegisterDto
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(2, ErrorMessage = "CharacterLess")]
        public string FirstName { get; set; }
        [DisplayName("Surname")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(2, ErrorMessage = "CharacterLess")]
        public string LastName { get; set; }
        [DisplayName("Username")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(50, ErrorMessage = "CharacterLarger")]
        [MinLength(6, ErrorMessage = "CharacterLess")]
        public string UserName { get; set; }
        [Display(Name = "Email",Prompt ="Email")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(100, ErrorMessage = "CharacterLarger")]
        [MinLength(10, ErrorMessage = "CharacterLess")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("PhoneNumber")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(18, ErrorMessage = "CharacterLarger")]
        [MinLength(11, ErrorMessage = "CharacterLess")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DisplayName("CompanyName")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(2000, ErrorMessage = "CharacterLarger")]
        [MinLength(3, ErrorMessage = "CharacterLess")]
        public string CompanyName { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(8, ErrorMessage = "CharacterLess")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("RePassword")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(8, ErrorMessage = "CharacterLess")]
        [Compare(nameof(Password), ErrorMessage = "PasswordNotMatch")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
        public string Token { get; set; }
    }
}
