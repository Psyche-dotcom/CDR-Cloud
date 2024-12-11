using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Shared.Entities.Abstract
{
    public abstract class EntityBase
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now; // override CreatedDate = new DateTime(2020/01/01);
        public virtual DateTime UpdatedDate { get; set; } =DateTime.Now;
        public virtual bool IsActive { get; set; } = true;
        public virtual string ModifiedUser { get; set; } = "Modified User";
    }
}
