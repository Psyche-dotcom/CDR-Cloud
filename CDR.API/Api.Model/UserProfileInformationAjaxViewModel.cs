using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class UserProfileInformationAjaxViewModel
    {
        public UserProfileInformationDto UserProfileInformationDto { get; set; }
        public string UserProfileInformationPartial { get; set; }
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}
