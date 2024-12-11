using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class UserTimezoneAjaxViewModel
    {
        public UserTimezoneDto UserTimezoneDto { get; set; }
        public string UserTimezonePartial { get; set; }
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}
