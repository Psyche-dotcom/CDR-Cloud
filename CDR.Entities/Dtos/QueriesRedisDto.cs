using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class QueriesRedisDto
    {
        public int UserID { get; set; }
        public string IpAddress { get; set; }
        public IList<QueriesRedisDataListDto> DataList { get; set; }
    }
}
