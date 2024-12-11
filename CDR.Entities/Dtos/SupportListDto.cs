using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class SupportListDto
    {
        public IList<Support> Supports { get; set; }
    }
}
