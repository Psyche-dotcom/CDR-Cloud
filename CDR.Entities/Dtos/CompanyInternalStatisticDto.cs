using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyInternalStatisticDto
    {
        public long totalext2ext { get; set; }
        public long answeredext2ext { get; set; }
        public long missedext2ext { get; set; }
    }
}
