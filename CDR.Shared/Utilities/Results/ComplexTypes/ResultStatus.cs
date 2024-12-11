using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Shared.Utilities.Results.ComplexTypes
{
    public enum ResultStatus
    {
        Success=0,
        Error=1,
        Warning = 2,
        Info = 3
    }

    public enum VERSION3CX
    {
        [Description("V16")]
        V16 = 1,
        [Description("V18")]
        V18,
    }
}
