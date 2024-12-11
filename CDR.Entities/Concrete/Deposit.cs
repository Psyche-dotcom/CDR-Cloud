using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Deposit : EntityBase, IEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }
        public Guid PublicId { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
