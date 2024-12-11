using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Localization : EntityBase, IEntity
    {
        public string ResourceKey { get; set; }
        public string Text { get; set; }
        public int CultureId { get; set; }
        public LocalizationCulture Culture { get; set; }
    }
}
