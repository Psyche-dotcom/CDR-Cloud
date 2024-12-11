using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class UserPasswordChangeAjaxViewModel
    {
        public UserPasswordChangeDto UserPasswordChangeDto { get; set; }
        public string UserPasswordChangePartial { get; set; }
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}