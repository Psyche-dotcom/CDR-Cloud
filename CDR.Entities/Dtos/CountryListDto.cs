﻿using CDR.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CountryListDto
    {
        public IList<Country> Countries { get; set; }
    }
}