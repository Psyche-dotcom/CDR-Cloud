using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class DashboardLeastTalkDto
    {
        public string d_name { get; set; }
        public TimeSpan duration { get; set; }
    }
}
