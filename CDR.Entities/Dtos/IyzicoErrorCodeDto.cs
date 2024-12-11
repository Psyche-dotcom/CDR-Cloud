using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class IyzicoErrorCodeDto
    {
        public string errorCode { get; set; }
        public string errorMessageTr { get; set; }
        public string errorMessageEn { get; set; }
        public string errorGroup { get; set; }
    }
}
