using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Country : EntityBase, IEntity
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
