namespace CDR.API.Api.Model
{
    public class CompanyTotalCallsModel
    {
        public IList<string> xAxis { get; set; } = new List<string>();
        public IList<long> InboundList { get; set; } = new List<long>();
        public IList<long> OutboundList { get; set; } = new List<long>();
        public IList<long> TotalList { get; set; } = new List<long>();
    }
}
