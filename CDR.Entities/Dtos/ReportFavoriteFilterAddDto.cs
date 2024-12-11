using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class ReportFavoriteFilterAddDto
    {
        public string Title { get; set; }
        public string Json { get; set; }
        public int UserId { get; set; }
    }
}
