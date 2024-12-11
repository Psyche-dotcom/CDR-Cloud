﻿using System;
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
    public class EfHelpRepository : EfEntityRepositoryBase<Help>, IHelpRepository
    {
        public EfHelpRepository(DbContext context) : base(context)
        {
        }

    }
}