﻿using CDR.Entities.Concrete;
using CDR.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class DepositDto : DtoGetBase
    {
        public Deposit Deposit { get; set; }
    }
}
