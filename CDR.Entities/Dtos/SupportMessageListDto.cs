using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class SupportMessageListDto
    {
        public IList<SupportMessages> SupportMessages { get; set; }
        public IList<int> NewMessages { get; set; }
    }
}
