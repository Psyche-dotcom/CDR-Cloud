using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyReportCallInformationDto
    {
        public int call_id { get; set; }
        public int seg_order { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public string durum { get; set; }
        public int act { get; set; }
        public TimeSpan ringortalktime { get; set; }

        public string starttimestring
        {
            get
            {
                return (this.start_time.ToString().Contains(".") ? this.start_time.ToString().Split(".")[1].ToString() : this.start_time.ToString());
            }
        }
        public string stoptimestring
        {
            get
            {
                return (this.end_time.ToString().Contains(".") ? this.end_time.ToString().Split(".")[1].ToString() : this.end_time.ToString());
            }
        }
    }
}
