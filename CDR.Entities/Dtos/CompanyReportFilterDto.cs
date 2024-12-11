using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyReportFilterDto
    {
        public string Title { get; set; }
        public int? Dates { get; set; }
        public DateTime? DatesStart { get; set; }
        public DateTime? DatesEnd { get; set; }
        public int? Source { get; set; }
        public int? SourceCriteria { get; set; }
        public string SourceCriteriaInput { get; set; }
        public int? Target { get; set; }
        public int? TargetCriteria { get; set; }
        public string TargetCriteriaInput { get; set; }
        public int? Status { get; set; }
        public int? Duration { get; set; }
    }
}
