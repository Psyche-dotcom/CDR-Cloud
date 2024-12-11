using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Permissions : EntityBase, IEntity
    {
        public string Title { get; set; }
        public string PermissionGroup { get; set; }
    }
}
