using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class UserLoginDto
    {
        [DisplayName("UsernameOrEmail")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(100, ErrorMessage = "CharacterLarger")]
        [MinLength(3, ErrorMessage = "CharacterLess")]
        public string Email { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(30, ErrorMessage = "CharacterLarger")]
        [MinLength(5, ErrorMessage = "CharacterLess")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
