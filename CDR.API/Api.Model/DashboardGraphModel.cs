namespace CDR.API.Api.Model
{
    public class DashboardGraphModel
    {
        public List<string> xAxis { get; set; } = new List<string>();
        public List<long> InboundList { get; set; } = new List<long>();
        public List<long> OutboundList { get; set; } = new List<long>();
        public List<long> TotalList { get; set; } = new List<long>();
    }
}
