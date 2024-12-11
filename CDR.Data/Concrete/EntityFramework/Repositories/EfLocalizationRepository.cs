using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDR.Data.Abstract;
using CDR.Entities.Concrete;
using CDR.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace CDR.Data.Concrete.EntityFramework.Repositories
{
    public class EfLocalizationRepository : EfEntityRepositoryBase<Localization>, ILocalizationRepository
    {
        public EfLocalizationRepository(DbContext context) : base(context)
        {
        }

    }
}
