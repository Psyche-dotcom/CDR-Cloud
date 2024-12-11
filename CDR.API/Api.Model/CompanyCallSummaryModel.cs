namespace CDR.API.Api.Model
{
    public class CompanyCallSummaryModel
    {
        public IList<string> dates { get; set; }
        public IList<long> inbound { get; set; }
        public IList<long> outbound { get; set; }
        public IList<long> missed { get; set; }
        public IList<long> abandoned { get; set; }
        public IList<long> ext2ext { get; set; }
    }
}
