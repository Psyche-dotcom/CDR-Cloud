using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class DashboardGraphDto
    {
        public int? day { get; set; }
        public int? month { get; set; }
        public int? years { get; set; }
        public long outbound { get; set; }
        public long inbound { get; set; }
        public long total { get; set; }
        public string yearsString
        {
            get
            {
                string _string = string.Empty;

                if (years != null && years != 0)
                {
                    var _temp = years.ToString();

                    if (_temp.Length == 4)
                    {
                        _string = _temp[2].ToString() + _temp[3].ToString();
                    }
                }

                return _string;
            }
        }
    }
}
