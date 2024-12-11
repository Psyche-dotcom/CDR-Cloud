using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class UserConnectionDetailAjaxViewModel
    {
        public UserConnectionDetailDto UserConnectionDetailDto { get; set; }
        public string UserConnectionDetailPartial { get; set; }
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}
