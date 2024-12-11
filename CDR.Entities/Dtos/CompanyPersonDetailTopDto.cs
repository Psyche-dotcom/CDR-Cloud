using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyPersonDetailTopDto
    {
        public string extnumber { get; set; }
        public int numofcalls { get; set; }
        public TimeSpan totaltalktime { get; set; }
        public int numofanswered { get; set; }
        public int numofmissed { get; set; }
    }
}
