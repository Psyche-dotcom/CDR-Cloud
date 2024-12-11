using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyCallSummaryDto
    {
        public string date { get; set; }
        public long totalinbound { get; set; }
        public long totaloutbound { get; set; }
        public long totalmissed { get; set; }
        public long totalabandoned { get; set; }
        public long totalext2ext { get; set; }
    }
}
