using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class DashboardTotalGraphDto
    {
        public DateTime datevalue { get; set; }
        public long totalcalls { get; set; }
        public long inbound { get; set; }
        public long outbound { get; set; }
        public long missed { get; set; }
        public long abandoned { get; set; }
        public long ext2ext { get; set; }
    }
}
