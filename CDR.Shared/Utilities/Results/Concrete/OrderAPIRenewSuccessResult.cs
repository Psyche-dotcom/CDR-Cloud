using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Shared.Utilities.Results.Concrete
{
    public class OrderAPIRenewSuccessResult
    {
        public string Edition { get; set; }
        public bool IsPerpetual { get; set; }
        public int SimultaneousCalls { get; set; }
        public object Extensions { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public float NetPrice { get; set; }
        public int Years { get; set; }
        public float YearPrice { get; set; }
        public float YearNetPrice { get; set; }
        public float DiscountPercent { get; set; }
        public float PriceWithoutHosting { get; set; }
        public float NetPriceWithoutHosting { get; set; }
        public float YearPriceWithoutHosting { get; set; }
        public float YearNetPriceWithoutHosting { get; set; }
        public float HostingDiscountPercent { get; set; }
        public int HostingPrice { get; set; }
        public float NetHostingPrice { get; set; }
        public int YearHostingPrice { get; set; }
        public float YearNetHostingPrice { get; set; }

    }
}
