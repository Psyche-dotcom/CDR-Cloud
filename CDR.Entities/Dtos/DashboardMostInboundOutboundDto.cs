using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class DashboardMostInboundOutboundDto
    {
        public string from { get; set; }
        public string to { get; set; }
        public long numberofcalls { get; set; }
    }
}
