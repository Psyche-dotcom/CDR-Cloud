namespace CDR.API.Api.Model
{
    public class GetPhoneBookRequestDto
    {
        public int? draw { get; set; }
        public int? start { get; set; }
        public int? length { get; set; }
        public string namesurname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
