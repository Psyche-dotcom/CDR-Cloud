using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Content : EntityBase, IEntity
    {
        public byte ContentType { get; set; }
        public int LocalizationCultureId { get; set; }
        public LocalizationCulture LocalizationCulture { get; set; }
        public string Text { get; set; }
    }
}
