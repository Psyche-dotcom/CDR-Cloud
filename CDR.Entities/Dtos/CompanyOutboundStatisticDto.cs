using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyOutboundStatisticDto
    {
        public long totaloutbound { get; set; }
        public long answeredoutbound { get; set; }
        public long outboundmissed { get; set; }
        public long outboundabandoned { get; set; }
    }
}
