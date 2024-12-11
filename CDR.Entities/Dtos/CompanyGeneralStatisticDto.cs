using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyGeneralStatisticDto
    {
        public TimeSpan totalcalltime { get; set; }
        public TimeSpan avrgcalltime { get; set; }
        public TimeSpan totaltalkduration { get; set; }
        public TimeSpan avrgtalkduration { get; set; }
        public TimeSpan totalringtime { get; set; }
        public TimeSpan avrgringtime { get; set; }
        public TimeSpan totalabandonedtime { get; set; }
        public TimeSpan avrgabandonedtime { get; set; }
        public long totalvoicemail { get; set; }
    }
}
