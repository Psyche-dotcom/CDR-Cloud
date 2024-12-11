using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Support : EntityBase, IEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public SupportCategories Category { get; set; }
        public Guid PublicId { get; set; }
        public byte Statue { get; set; }
        public ICollection<SupportMessages> SupportMessages { get; set; }
    }
}
