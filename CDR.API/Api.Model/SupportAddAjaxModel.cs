using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class SupportAddAjaxModel
    {
        public SupportAddDto SupportAddDto { get; set; }
        public string SupportAddPartial { get; set; }
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}
