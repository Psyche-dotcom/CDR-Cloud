using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class SupportCategories : EntityBase, IEntity
    {
        public string Name { get; set; }
        public bool IsShown { get; set; }
        public Guid PublicId { get; set; }
        public ICollection<Support> Supports { get; set; }
    }
}
