﻿using CDR.Data.Abstract;
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
    public class EfCityRepository : EfEntityRepositoryBase<City>, ICityRepository
    {
        public EfCityRepository(DbContext context) : base(context)
        {
        }
    }
}