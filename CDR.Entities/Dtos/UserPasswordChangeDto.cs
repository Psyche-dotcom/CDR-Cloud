using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class UserPasswordChangeDto
    {
        [DisplayName("CurrentPassword")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(8, ErrorMessage = "CharacterLess")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DisplayName("NewPassword")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(8, ErrorMessage = "CharacterLess")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DisplayName("RepeatNewPassword")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(8, ErrorMessage = "CharacterLess")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "PasswordNotMatch")]
        public string RepeatPassword { get; set; }
        public string? ModelError { get; set; }
    }
}
