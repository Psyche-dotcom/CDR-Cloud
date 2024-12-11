using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class IyzicoSettings
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string BaseUrl { get; set; }
        public string DomainUrl { get; set; }
        public string CallSubpartUrl { get; set; }
    }
}
