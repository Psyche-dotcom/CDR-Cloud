using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Queries : EntityBase, IEntity
    {
        public string JsonData { get; set; }
        public int Type { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
    }
}
