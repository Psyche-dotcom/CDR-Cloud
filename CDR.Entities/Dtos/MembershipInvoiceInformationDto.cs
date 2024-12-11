using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class MembershipInvoiceInformationDto
    {
        [DisplayName("Country")]
        [Required(ErrorMessage = "RequiredMessage")]
        public int? CountryId { get; set; }
        [DisplayName("City")]
        [Required(ErrorMessage = "RequiredMessage")]
        public long? CityId { get; set; }
        [DisplayName("Address")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string Address { get; set; }
        [DisplayName("ZipCode")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string ZipCode { get; set; }
    }
}
