using CDR.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class SupportDto
    {
        public int Id { get; set; }
        public long SupportNumber
        {
            get
            {
                return 10000 + Id;
            }
        }
        public Guid SupportPublicId { get; set; }
        public string SupportCategories { get; set; }
        public string UserNameSurname { get; set; }
        public Enums.SupportStatue Statue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
        public string RegexMessage
        {
            get
            {
                string _returnText = Message;

                if (!string.IsNullOrWhiteSpace(Message))
                    _returnText = Message.Replace("\n", "<br>");

                return _returnText;
            }
        }
        public string CreatedDateString
        {
            get
            {
                return string.Format("{0:dd.MM.yyyy}", CreatedDate);
            }
        }
        public string Parameter { get; set; }
        public bool NoSeen { get; set; }
    }
}
