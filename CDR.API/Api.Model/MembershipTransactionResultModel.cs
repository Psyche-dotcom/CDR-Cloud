using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class MembershipTransactionResultModel
    {
        public string Statue { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public string Message { get; set; }
    }
}
