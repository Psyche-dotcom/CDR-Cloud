﻿using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class ReportFavoriteFilterListDto
    {
        public IList<ReportFilter> ReportFilters { get; set; }
    }
}