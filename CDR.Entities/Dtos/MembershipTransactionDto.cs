using CDR.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class MembershipTransactionDto
    {
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public byte Currency { get; set; }
        public string PriceString
        {
            get
            {
                return this.Price.ToNumberString(suffix: Shared.Utilities.Extensions.BaseExtensions.CurrencyIcon((Enums.Currency)this.Currency), countAfterCommas: 2);
            }
        }
        public string TransactionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString
        {
            get
            {
                return this.CreatedDate.ToDatatableDateString();
            }
        }
    }
}
