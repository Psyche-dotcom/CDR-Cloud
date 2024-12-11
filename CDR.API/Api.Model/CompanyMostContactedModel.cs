using CDR.Entities.Dtos;

namespace CDR.API.Api.Model
{
    public class CompanyMostContactedModel
    {
        public IList<CompanyMostContactedDto> DataList { get; set; }
        public List<string> DataNameList { get; set; }
    }
}
