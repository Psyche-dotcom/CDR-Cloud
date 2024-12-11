using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyPersonDto
    {
        public string dn { get; set; }
        public string display_name { get; set; }
        public long totalinbound { get; set; }
        public long totaloutbound { get; set; }
        public long totalmissed { get; set; }
    }
}
