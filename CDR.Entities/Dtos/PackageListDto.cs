using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class PackageListDto
    {
        public IList<Package> Packages { get; set; }
    }
}
