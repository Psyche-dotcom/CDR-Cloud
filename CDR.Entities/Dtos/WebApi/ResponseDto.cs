using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos.WebApi
{

    public class ResponseDto<T>
    {
        public int StatusCode { get; set; }
        public string DisplayMessage { get; set; }
        public T Result { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
