using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos.WebApi.ViewModel
{
    public class ConfirmEmailRequestModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
