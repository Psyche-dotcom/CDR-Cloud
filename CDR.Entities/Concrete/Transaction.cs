using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Concrete
{
    public class Transaction : EntityBase, IEntity
    {
        public decimal Price { get; set; }
        public string TransactionId { get; set; }
        public Guid PublicId { get; set; }
        public Guid DepositPublicId { get; set; }
        public Deposit Deposit { get; set; }
        public byte Currency { get; set; }
    }
}
