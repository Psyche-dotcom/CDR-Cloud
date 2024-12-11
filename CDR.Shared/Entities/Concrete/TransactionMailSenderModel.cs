using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Shared.Entities.Concrete
{
    public class TransactionMailSenderModel
    {
        public string PdfHtmlText { get; set; }
        public string MailText { get; set; }
        public string EmailSubject { get; set; }
        public string EmailReceiverCustomerMail { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
