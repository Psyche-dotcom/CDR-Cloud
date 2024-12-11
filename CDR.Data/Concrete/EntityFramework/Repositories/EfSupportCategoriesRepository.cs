using CDR.Data.Abstract;
using CDR.Entities.Concrete;
using CDR.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Data.Concrete.EntityFramework.Repositories
{
    public class EfSupportCategoriesRepository:EfEntityRepositoryBase<SupportCategories>, ISupportCategoriesRepository
    {
        public EfSupportCategoriesRepository(DbContext context) : base(context)
        {
        }
    }
}
