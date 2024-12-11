using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class DepositDetailDto
    {
        public Guid DepositPublicId { get; set; }
        public string PackageName { get; set; }
        public decimal MontlyPrice { get; set; }
        public decimal YearPrice { get; set; }
        public byte Month { get; set; }
        public byte Currency { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
    }
}
