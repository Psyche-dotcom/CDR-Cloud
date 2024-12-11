using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Help : EntityBase, IEntity
    {
        public string LocalizationKey { get; set; }
        public byte PageType { get; set; }
        public ICollection<HelpDetails> HelpDetails { get; set; }
    }
}
