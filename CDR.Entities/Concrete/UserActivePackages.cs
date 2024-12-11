using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class UserActivePackages : EntityBase, IEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
