using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class UserPermissions : EntityBase, IEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PermissionId { get; set; }
        public Permissions Permission { get; set; }
    }
}
