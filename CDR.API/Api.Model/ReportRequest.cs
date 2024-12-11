namespace CDR.API.Api.Model
{
    public class ReportRequest
    {
        public int? draw { get; set; }
        public int? start { get; set; }
        public int? length { get; set; }
        public string? json { get; set; }  // Use nullable if the JSON string might be null
        public string? ReportDate { get; set; }
        public long ReportCount { get; set; }
    }
}
