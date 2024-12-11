using CDR.Data.Abstract;
using CDR.Entities.Concrete;
using CDR.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace CDR.Data.Concrete.EntityFramework.Repositories
{
    public class EfPromotionUsageRepository : EfEntityRepositoryBase<PromotionUsage>, IPromotionUsageRepository
    {
        public EfPromotionUsageRepository(DbContext context) : base(context)
        {
        }
    }
}
