using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class UserForgotPasswordDto
    {
        [Display(Name = "Email", Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(100, ErrorMessage = "CharacterLarger")]
        [MinLength(10, ErrorMessage = "CharacterLess")]
        public string Email { get; set; }
    }
}
