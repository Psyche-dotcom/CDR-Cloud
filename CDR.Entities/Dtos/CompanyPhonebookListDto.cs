using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyPhonebookListDto
    {
        public IList<CompanyPhonebookUserDto> Users{ get; set; }
    }
}
