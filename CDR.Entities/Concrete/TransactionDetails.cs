using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class TransactionDetails : EntityBase, IEntity
    {
        public string paymentStatus { get; set; }
        public string paymentId { get; set; }
        public string price { get; set; }
        public string paidPrice { get; set; }
        public string currency { get; set; }
        public string binNumber { get; set; }
        public string lastFourDigits { get; set; }
        public string cardAssociation { get; set; }
        public string cardFamily { get; set; }
        public string cardType { get; set; }
        public string fraudStatus { get; set; }
        public string paymentTransactionId { get; set; }
        public string mdStatus { get; set; }
    }
}
