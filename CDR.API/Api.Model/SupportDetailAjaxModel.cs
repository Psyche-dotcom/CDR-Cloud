namespace CDR.API.Api.Model
{
    public class SupportDetailAjaxModel
    {
        public byte ResultStatus { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public long SupportNumber
        {
            get
            {
                return 100000 + Id;
            }
        }
        public Guid SupportPublicId { get; set; }
        public string SupportCategories { get; set; }
        public byte Statue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString
        {
            get
            {
                return string.Format("{0:dd.MM.yyyy}", CreatedDate);
            }
        }
    }
}

