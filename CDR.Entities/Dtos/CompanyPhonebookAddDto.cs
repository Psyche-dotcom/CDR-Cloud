using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyPhonebookAddDto
    {
        public int? idphonebook { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(100, ErrorMessage = "CharacterLarger")]
        [MinLength(3, ErrorMessage = "CharacterLess")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Company { get; set; }
    }
}
