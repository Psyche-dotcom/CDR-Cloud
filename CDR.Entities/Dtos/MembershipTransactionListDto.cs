﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class MembershipTransactionListDto
    {
        public IList<MembershipTransactionDto> Transactions { get; set; }
    }
}
