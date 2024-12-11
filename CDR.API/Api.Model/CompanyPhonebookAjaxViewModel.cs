using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class CompanyPhonebookAjaxViewModel
    {
        public CompanyPhonebookAddDto CompanyPhonebookAddDto { get; set; }
        public string CompanyPhonebookManagementPartial { get; set; }
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}
