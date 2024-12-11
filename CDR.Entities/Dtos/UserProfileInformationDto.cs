using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class UserProfileInformationDto
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
        [Display(Name = "Email", Prompt = "Email")]
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
        public int? CountryId { get; set; }
        public long? CityId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }

        public string? ModelError { get; set; }
    }
}
