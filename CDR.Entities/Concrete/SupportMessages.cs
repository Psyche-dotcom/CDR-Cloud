using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class SupportMessages : EntityBase, IEntity
    {
        public int SupportId { get; set; }
        public Support Support { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSeenAdmin { get; set; }
        public bool IsSeenUser { get; set; }
        public Guid PublicId { get; set; }
    }
}
