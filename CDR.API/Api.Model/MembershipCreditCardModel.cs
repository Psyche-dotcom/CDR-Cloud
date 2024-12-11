using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;

namespace CDR.API.Api.Model
{
    public class MembershipCreditCardModel
    {
        public DepositDetailDto DepositDetail { get; set; }
        public string Message { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public string CheckoutForm { get; set; }
        public string ExhangeRate { get; set; }
    }
}
