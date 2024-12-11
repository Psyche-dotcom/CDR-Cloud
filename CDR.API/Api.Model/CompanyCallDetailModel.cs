using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class CompanyCallDetailModel
    {
        public Entities.Dtos.CompanyReportCallDetailDto Detail { get; set; }
        public IList<Entities.Dtos.CompanyReportCallInformationDto> Information { get; set; }
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
    }
}
