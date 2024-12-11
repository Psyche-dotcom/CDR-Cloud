using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyInboundStatisticDto
    {
        public long totalinbound { get; set; }
        public long answeredinbound { get; set; }
        public long unansweredInbound { get; set; }
    }
}
