using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class ReportFilter : EntityBase, IEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string Json { get; set; }
    }
}
