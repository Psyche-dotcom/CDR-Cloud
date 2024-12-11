using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class HelpDetails : EntityBase, IEntity
    {
        public int HelpId { get; set; }
        public Help Help { get; set; }
        public string Element { get; set; }
        public string LocalizationKey { get; set; }
        public string JsText { get; set; }
        public byte Type { get; set; }
    }
}
