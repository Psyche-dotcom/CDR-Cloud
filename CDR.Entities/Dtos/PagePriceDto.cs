using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class PagePriceDto
    {
        public PackageListDto Monthly { get; set; }
        public PackageListDto Annual { get; set; }
        public bool Type { get; set; } //True ise Annual
    }
}
