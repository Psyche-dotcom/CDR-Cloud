using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class DepositAddDto
    {
        [DisplayName("Package")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string PackagePublicId { get; set; }
        public User User { get; set; }
    }
}
