using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class LocalizationRedisDto
    {
        public string ResourceKey { get; set; }
        public IList<Localization> DataList { get; set; }
    }
}
