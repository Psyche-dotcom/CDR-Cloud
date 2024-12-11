using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class LocalizationCulture : EntityBase, IEntity
    {
        public string Title { get; set; }
        public string Culture { get; set; }
        public string FlagIcon { get; set; }
        public ICollection<Content> Contents { get; set; }
        public ICollection<Localization> Localizations { get; set; }
    }
}
