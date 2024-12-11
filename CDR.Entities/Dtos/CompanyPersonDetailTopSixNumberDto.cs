using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyPersonDetailTopSixNumberDto
    {
        public string extnumber { get; set; }
        public string phonenumber { get; set; }
        public string displayname { get; set; }
        public long numofcalls { get; set; }
    }
}
