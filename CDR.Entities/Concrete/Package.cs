using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Package : EntityBase, IEntity
    {
        public string Name { get; set; }
        public decimal MonthPrice { get; set; }
        public decimal YearPrice { get; set; }
        public byte Currency { get; set; }
        public byte Month { get; set; }
        public bool IsTrial { get; set; }
        public Guid PublicId { get; set; }
        public bool IsShown { get; set; }
        public int SimultaneousCalls { get; set; }
        public ICollection<Deposit> Deposits { get; set; }
        public ICollection<UserActivePackages> UserPackages { get; set; }
    }
}
