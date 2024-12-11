using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class SupportAddDto
    {
        [DisplayName("SupportCategory")]
        [Required(ErrorMessage = "RequiredMessage")]
        public int? SupportCategoryId { get; set; }
        [DisplayName("SupportMessage")]
        [Required(ErrorMessage = "RequiredMessage")]
        [MaxLength(10000, ErrorMessage = "CharacterLarger")]
        public string SupportMessage { get; set; }
        public string ModelError { get; set; }
    }
}
