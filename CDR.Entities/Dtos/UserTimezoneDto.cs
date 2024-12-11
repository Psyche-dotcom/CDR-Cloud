using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class UserTimezoneDto
    {
        [DisplayName("GMT")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string GMT { get; set; }

        [DisplayName("Timezone")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string Timezone { get; set; }

        public string? ModelError { get; set; }
    }
}
