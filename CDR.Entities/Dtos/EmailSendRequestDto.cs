using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class EmailSendRequestDto
    {
        /// <summary>
        /// sender email address
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// recipient email addresses. max 50. For multiple addresses, send as an array of string.
        /// </summary>
        public List<string> to { get; set; }

        /// <summary>
        /// email subject
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// bcc recipient email address
        /// </summary>
        public string bcc { get; set; }

        /// <summary>
        /// Cc recipient email address. For multiple addresses, send as an array of strings.
        /// </summary>
        public string cc { get; set; }

        /// <summary>
        /// Reply-to email address. For multiple addresses, send as an array of strings.
        /// </summary>
        public List<string> reply_to { get; set; }

        /// <summary>
        /// The HTML version of the message.
        /// </summary>
        public string html { get; set; }

        /// <summary>
        /// The plain text version of the message.
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// The React component used to write the message. Only available in the Node.js SDK.
        /// </summary>
        public string react { get; set; }

        /// <summary>
        /// Custom headers to add to the email.
        /// </summary>
        public object headers { get; set; }

        /// <summary>
        /// Filename and content of attachments (max 40mb per email)
        /// </summary>
        public List<Attachment> attachments { get; set; }

        /// <summary>
        /// Email tags
        /// </summary>
        public List<Tag> tags { get; set; }
    }

    public class Tag
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
