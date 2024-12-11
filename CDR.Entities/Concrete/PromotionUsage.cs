using CDR.Shared.Entities.Abstract;
using System;

namespace CDR.Entities.Concrete
{
    public class PromotionUsage : EntityBase, IEntity
    {
        public int UserId { get; set; }
        public string PromotionCode { get; set; }
        public string LicenseKey { get; set; }
    }
}
