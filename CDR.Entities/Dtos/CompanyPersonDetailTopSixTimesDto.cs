using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyPersonDetailTopSixTimesDto
    {
        public string extnumber { get; set; }
        public string phonenumber { get; set; }
        public string displayname { get; set; }
        public TimeSpan talkduration { get; set; }
    }
}
