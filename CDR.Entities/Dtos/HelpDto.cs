using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class HelpDto
    {
        public string LocalizationKey { get; set; }
        public byte PageType { get; set; }
        public IList<HelpDetailDto> HelpDetails { get; set; }
    }
}
