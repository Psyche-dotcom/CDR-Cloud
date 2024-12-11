using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class GlobalSettings
    {
        public string HangfireUrl { get; set; }
        public string DockerEnvironment { get; set; }
        public string DockerServer { get; set; }
    }
}
