using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Shared.Utilities.Results.Concrete
{
    public class TransactionPDFModel
    {
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMail { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Tax { get; set; }
        public string Total { get; set; }
        public string SubTotal { get; set; }
        public string Currency { get; set; }
        public string Period { get; set; }

    }
}
